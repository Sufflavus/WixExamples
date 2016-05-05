using System.Windows;
using System.Windows.Controls;

namespace CustomBA
{
    public class MainWindowDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GreetingPageTemplate { get; set; }

        public DataTemplate RepairAndUninstallGreetingPageTemplate { get; set; }

        public DataTemplate LicensePageTemplate { get; set; }

        public DataTemplate ParametersPageTemplate { get; set; }

        public DataTemplate InstallProgressPageTemplate { get; set; }

        public DataTemplate UninstallProgressPageTemplate { get; set; }

        public DataTemplate FinishPageTemplate { get; set; }


        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Windows.DataTemplate"/> based on custom logic.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate"/> or null. The default value is null.
        /// </returns>
        /// <param name="item">The data object for which to select the template.</param><param name="container">The data-bound object.</param>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }
    }
}