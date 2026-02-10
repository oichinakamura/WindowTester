using HIMTools.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HIMTools.Controls
{
    using SysCtrl = System.Windows.Controls;
    public class GridLayoutPanel : Grid
    {
        public GridLayoutPanel()
        {

        }

        public float[] ColumnDistributions
        {
            get => null;
            set
            {
                for (int n = 0; n < value.Length; n++)
                    if (ColumnDefinitions.Count > n && value[n] != 0)
                    {
                        if (value[n] > 0)
                            ColumnDefinitions[n].Width = new GridLength(value[n], GridUnitType.Star);
                        else
                            ColumnDefinitions[n].Width = GridLength.Auto;
                    }
                    else
                        return;
            }
        }

        public int ColumnCount
        {
            get => ColumnDefinitions.Count;
            set
            {
                if (value < 1 || ColumnDefinitions.Count == value)
                    return;
                while (ColumnDefinitions.Count < value)
                {
                    ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
                while (ColumnDefinitions.Count > value)
                {
                    ColumnDefinitions.Remove(ColumnDefinitions[ColumnDefinitions.Count - 1]);
                }
            }
        }
        public int RowCount
        {
            get => RowDefinitions.Count;
        }


        private HasEventUIElementCollection hasEventUIElementCollection;
        protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
        {
            hasEventUIElementCollection = new HasEventUIElementCollection(this, logicalParent);
            hasEventUIElementCollection.CollectionChanged += (s, e) =>
            {
                int x = 0;
                int y = 0;
                RowDefinitions.Clear();
                for (int i = 0; i < hasEventUIElementCollection.Count; i++)
                    if (hasEventUIElementCollection[i] is UIElement uIElement)
                    {
                        Grid.SetColumn(uIElement, x);
                        x++;
                        if (x > ColumnCount)
                        {
                            x = 0;
                            y++;
                            if (y > RowCount - 1)
                            {
                                RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                            }
                        }
                        Grid.SetRow(uIElement, y);
                    }
            };


            return hasEventUIElementCollection;
        }
    }
}
