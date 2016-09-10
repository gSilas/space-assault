using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceAssault.Utils
{
    class OptionMenuEntry : MenuEntry
    {
        public OptionMenuEntry(string text) : base(text)
        {
        }

        /// <summary>
        /// Event raised when the menu entry is increased
        /// </summary>
        public event EventHandler<EventArgs> SelectedIncrease;

        // Method for raising the Selected event.
        protected internal virtual void OnSelectEntryIncrease()
        {
            SelectedIncrease?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event raised when the menu entry is increased
        /// </summary>
        public event EventHandler<EventArgs> SelectedDecrease;

        // Method for raising the Selected event.
        protected internal virtual void OnSelectEntryDecrease()
        {
            SelectedDecrease?.Invoke(this, EventArgs.Empty);
        }
    }
}
