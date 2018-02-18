using System.Web.UI;
using System.Web.UI.HtmlControls;


namespace ASP.Helpers
{
    public static class GraphLabsUIFactory
    {
        public static HtmlGenericControl createHtmlForm()
        {
            var form = new HtmlGenericControl("form");
            form.Attributes.Add("class", "graphlabs-form");
            return form;
        }

        public static HtmlInputText createHtmlInputText()
        {
            var text = new HtmlInputText("text");
            text.Attributes.Add("class", "graphlabs-input-text");
            return text;
        }
        
        public static HtmlInputCheckBox createHtmlInputCheckBox()
        {
            var checkBox = new HtmlInputCheckBox();
            checkBox.Attributes.Add("class", "graphlabs-checkbox");
            return checkBox;
        }
        
        public static HtmlGenericControl createHtmlLabel()
        {
            var label = new HtmlGenericControl("label");
            label.Attributes.Add("class", "graphlabs-label");
            return label;
        }
        
        public static HtmlInputButton createHtmlSubmitButton()
        {
            var button = new HtmlInputButton("submit");
            button.Attributes.Add("class", "graphlabs-submit");
            button.Value = "Применить";
            return button;
        }
        
        public static HtmlInputButton createHtmlResetButton()
        {
            var button = new HtmlInputButton("reset");
            button.Attributes.Add("class", "graphlabs-reset");
            button.Value = "Очистить";
            return button;
        }

        public static Control createInputField(string name, string labelText)
        {
            var input = GraphLabsUIFactory.createHtmlInputText();
            input.Name = name;
            input.ID = name;
            
            var label = GraphLabsUIFactory.createHtmlLabel();
            label.InnerText = labelText;

            var div = new HtmlGenericControl("div");
            div.Controls.Add(label);
            label.Controls.Add(input);
            return div;
        }
    }
}