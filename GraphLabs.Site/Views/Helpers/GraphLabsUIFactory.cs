using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;


namespace ASP.Helpers
{
    public static class GraphLabsUIFactory
    {
        public static HtmlGenericControl CreateHtmlForm()
        {
            var form = new HtmlGenericControl("form");
            form.Attributes.Add("class", "graphlabs-form");
            return form;
        }

        public static HtmlInputText CreateHtmlInputText()
        {
            var text = new HtmlInputText("text");
            text.Attributes.Add("class", "graphlabs-input-text");
            return text;
        }

        public static HtmlInputCheckBox CreateHtmlInputCheckBox()
        {
            var checkBox = new HtmlInputCheckBox();
            checkBox.Attributes.Add("class", "graphlabs-checkbox");
            checkBox.Value = "";
            return checkBox;
        }

        public static HtmlGenericControl CreateHtmlLabel(string labelText)
        {
            var label = new HtmlGenericControl("label");
            label.Attributes.Add("class", "graphlabs-label");
            label.InnerText = labelText;
            return label;
        }

        public static HtmlInputButton CreateHtmlSubmitButton()
        {
            var button = new HtmlInputButton("submit");
            button.Attributes.Add("class", "graphlabs-submit");
            button.Value = "Применить";
            return button;
        }

        public static HtmlInputButton CreateHtmlResetButton()
        {
            var button = new HtmlInputButton("reset");
            button.Attributes.Add("class", "graphlabs-reset");
            button.Value = "Очистить";
            return button;
        }

        public static Control CreateInputField(string name, string labelText)
        {
            var input = CreateHtmlInputText();
            input.Name = name;
            input.ID = name;

            var label = CreateHtmlLabel(labelText);

            var div = CreateDiv();
            div.Controls.Add(label);
            label.Controls.Add(input);
            return div;
        }

        public static Control CreateInputCheckBox(string name, string labelText)
        {
            var input = CreateHtmlInputCheckBox();
            input.Name = name;
            input.ID = name;
            input.Value = "true";
            input.Attributes.Add("onchange", "checkboxChange(this)");

            HtmlGenericControl script = new HtmlGenericControl("script");
            script.InnerHtml = "function checkboxChange(chkBox) {  if (chkBox.value==\"true\") {chkBox.value= \"false\"; chkBox.indeterminate = true;} else { chkBox.value= \"true\"; chkBox.checked=true; } }";

            var label = CreateHtmlLabel(labelText);

            var div = CreateDiv();
            div.Controls.Add(label);

            label.Controls.Add(input);
            label.Controls.Add(script);
            return div;
        }

        public static Control CreateDiv()
        {
            return new HtmlGenericControl("div");
        }

        public static Control CreateSelectField(string name, string desc, Dictionary<string, string> options)
        {
            var div = CreateDiv();
            var label = CreateHtmlLabel(desc);
            
            var select = new HtmlGenericControl("select");
            select.Attributes.Add("name", name);
            foreach (var option in options)
            {
                select.Controls.Add(CreateHtmlOption(option.Key, option.Value));
            }
            
            div.Controls.Add(label);
            div.Controls.Add(select);
            return div;
        }

        private static Control CreateHtmlOption(string value, string desc)
        {
            var option = new HtmlGenericControl("option");
            option.Attributes.Add("value", value);
            option.InnerText = desc;
            return option;
        }
    }
}