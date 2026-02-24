using System.Windows;
using System.Windows.Controls;

namespace HIMTools.Controls
{
    public class GridLayoutPanel : Grid
    {
        public GridLayoutPanel()
        {

        }

        public object[] ColumnDistributions
        {
            get => null;
            set
            {
                for (int n = 0; n < value.Length; n++)
                    if (ColumnDefinitions.Count > n)
                    {
                        switch (value[n])
                        {
                            case float width:
                                if (width > 0)
                                    ColumnDefinitions[n].Width = new GridLength(width, GridUnitType.Star);
                                else
                                    ColumnDefinitions[n].Width = GridLength.Auto;
                                break;
                            case "Auto": ColumnDefinitions[n].Width = GridLength.Auto; break;
                            case "*": ColumnDefinitions[n].Width = new GridLength(1.0, GridUnitType.Star); break;
                            case string star:
                                if (star.EndsWith("*") && float.TryParse(star.Replace("*", ""), out float floatValue))
                                    ColumnDefinitions[n].Width = new GridLength(floatValue, GridUnitType.Star);
                                else if (float.TryParse(star.Replace("*", ""), out float floatRValue))
                                    ColumnDefinitions[n].Width = new GridLength(floatRValue, GridUnitType.Pixel);
                                break;
                        }
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
