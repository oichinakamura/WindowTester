namespace HIMTools.Controls
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;
    using SysCtrl = System.Windows.Controls;

    public partial class RichTextBox : SysCtrl.RichTextBox
    {
        public RichTextBox()
        {
            Bindings = new BindingCollection(this);
        }
        public BindingCollection Bindings { get; private set; }
        public void LoadText(string filePath)
        {
            if (!File.Exists(filePath)) return;

            string text = File.ReadAllText(filePath);

            FlowDocument doc = new FlowDocument();
            Paragraph p = new Paragraph(new Run(text));
            doc.Blocks.Add(p);

            Document = doc;
        }

        public virtual string Text
        {
            get => Document is null ? "" : new TextRange(Document.ContentStart, Document.ContentEnd).Text;
            set
            {

            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(RichTextBox),
            new FrameworkPropertyMetadata(null, OnTextChanged));
        public static void OnTextChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox richTextBox)
            {
                FlowDocument doc = new FlowDocument();
                doc.Blocks.Add(new Paragraph(new Run($"{e.NewValue}")));
                richTextBox.Document = doc;

                SearchText(richTextBox, "<Entry ", Brushes.Blue, Brushes.Yellow);
            }
        }

        private static void SearchText(RichTextBox rtb, string textToFind, Brush forground = null, Brush backround = null)
        {
            if (string.IsNullOrEmpty(textToFind)) return;

            TextPointer position = rtb.Document.ContentStart;

            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string text = position.GetTextInRun(LogicalDirection.Forward);
                    int index = text.IndexOf(textToFind, StringComparison.CurrentCultureIgnoreCase);

                    if (index >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(index);
                        TextPointer end = start.GetPositionAtOffset(textToFind.Length);

                        TextRange selection = new TextRange(start, end);
                        if (forground != null)
                            selection.ApplyPropertyValue(TextElement.ForegroundProperty, forground);
                        if (backround != null)
                            selection.ApplyPropertyValue(TextElement.BackgroundProperty, backround);
                    }
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
        }
    }
}
