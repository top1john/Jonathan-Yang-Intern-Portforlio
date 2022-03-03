using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using starTEDSystem.BLL;
using starTEDSystem.Entities;
using WebApp.Helpers;

namespace WebApp.Pages
{
    public class QueryModel : PageModel
    {
        #region Private service fields, FeedBackMessage & constructor (dependency injection)

        private readonly ProgramServices _programservices;


        public QueryModel(ProgramServices programservices)
        {

            _programservices = programservices;
        }

        [TempData]
        public string FeedbackMessage { get; set; }
        #endregion

        #region Web Page variable and data
        [BindProperty(SupportsGet = true)]
        public string searcharg { get; set; }

        [BindProperty]
        public List<SchoolProgram> programInfo { get; set; }

        public List<School> SchoolList { get; set; }

        #endregion


        #region Paginator
        //my desired page size
        private const int PAGE_SIZE = 10;
        //instance of the Paginator
        public Paginator Pager { get; set; }
        #endregion


        public void OnGet(int? currentPage)
        {

            if (!string.IsNullOrWhiteSpace(searcharg))
            {

                int pagenumber = currentPage.HasValue ? currentPage.Value : 1;

                PageState current = new(pagenumber, PAGE_SIZE);

                int totalcount;

                SchoolList = _programservices.School_List();

                programInfo = _programservices.Program_GetByPartialDescription(searcharg, pagenumber, PAGE_SIZE,
                    out totalcount);

                Pager = new Paginator(totalcount, current);
            }
        }

        public IActionResult OnPostByName()
        {

            if (string.IsNullOrWhiteSpace(searcharg))
            {
                FeedbackMessage = "Enter a program name before searching";
            }
            return RedirectToPage(new { searcharg = searcharg });
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/CRUD");
        }
    }
}
