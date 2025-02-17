﻿// Copyright (c) Umbraco.
// See LICENSE for more details.

using System.Collections.Generic;
using System.Threading;
using Umbraco.Cms.Core.Actions;
using Umbraco.Cms.Core.Models.Trees;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Umbraco.Cms.Core.Trees
{
    /// <summary>
    /// A custom menu list
    /// </summary>
    /// <remarks>
    /// NOTE: We need a sub collection to the MenuItemCollection object due to how json serialization works.
    /// </remarks>
    public class MenuItemList : List<MenuItem>
    {
        private readonly ActionCollection _actionCollection;

        public MenuItemList(ActionCollection actionCollection)
        {
            _actionCollection = actionCollection;
        }

        public MenuItemList(ActionCollection actionCollection, IEnumerable<MenuItem> items)
            : base(items)
        {
            _actionCollection = actionCollection;
        }

        /// <summary>
        /// Adds a menu item with a dictionary which is merged to the AdditionalData bag
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hasSeparator"></param>
        /// <param name="textService">The <see cref="ILocalizedTextService"/> used to localize the action name based on its alias</param>
        /// <param name="opensDialog">Whether or not this action opens a dialog</param>
        public MenuItem Add<T>(ILocalizedTextService textService, bool hasSeparator = false, bool opensDialog = false)
            where T : IAction
        {
            var item = CreateMenuItem<T>(textService, hasSeparator, opensDialog);
            if (item != null)
            {
                Add(item);
                return item;
            }
            return null;
        }

        private MenuItem CreateMenuItem<T>(ILocalizedTextService textService, bool hasSeparator = false, bool opensDialog = false)
            where T : IAction
        {
            var item = _actionCollection.GetAction<T>();
            if (item == null) return null;

            var values = textService.GetAllStoredValues(Thread.CurrentThread.CurrentUICulture);
            values.TryGetValue($"visuallyHiddenTexts/{item.Alias}Description", out var textDescription);

            var menuItem = new MenuItem(item, textService.Localize($"actions", item.Alias))
            {
                SeparatorBefore = hasSeparator,
                OpensDialog = opensDialog,
                TextDescription = textDescription,
            };

            return menuItem;
        }
    }
}
