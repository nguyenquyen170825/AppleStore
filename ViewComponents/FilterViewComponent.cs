using Microsoft.AspNetCore.Mvc;

namespace DUANCUAHANGAPPLE.ViewComponents
{
    public class FilterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string title, List<string> options)
        {
            var model = new FilterModel
            {
                Title = title,
                Options = options
            };

            return View(model);
        }
    }

    public class FilterModel
    {
        public string Title { get; set; }
        public List<string> Options { get; set; }
    }
}