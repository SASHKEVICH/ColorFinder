using System.Collections.ObjectModel;

namespace ColorFinder.Models
{
    public class ColorInterpretation : ObservableCollection<string>
    {
        public ColorInterpretation()
        {
            Add("Rgb");
            Add("Hex");
        }
    }
}