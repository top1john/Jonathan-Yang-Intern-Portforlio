using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using starTEDSystem.BLL;
using starTEDSystem.Entities;

namespace WebApp.Pages
{
    public class CRUDModel : PageModel
    {
        #region Private service fields, FeedBackMessage & constructor (dependency injection)

        private readonly ProgramServices _programservices;


        public CRUDModel(ProgramServices programservices)
        {

            _programservices = programservices;
        }
        #endregion
        
        [TempData]
        public string FeedbackMessage { get; set; }
        public bool HasFeedback => !string.IsNullOrWhiteSpace(FeedbackMessage);

        [TempData]
        public string ErrorMessage { get; set; }

        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

        [BindProperty(SupportsGet = true)]
        public int? programId { get; set; }

        [BindProperty(SupportsGet = true)]
        public SchoolProgram SchoolProgramInfo { get; set; }

        [BindProperty]
        public List<SchoolProgram> programInfo { get; set; }

        public List<School> SchoolList { get; set; }



        public void OnGet()
        {
            if (programId !=null)
            {
                SchoolProgramInfo = _programservices.Program_GetByProgramID((int)programId);
            }
            SchoolList = _programservices.School_List();

        }

        public IActionResult OnPostNew()
        {
            if (ModelState.IsValid)
            {

                try
                {
                    _programservices.Program_Add(SchoolProgramInfo);
                    FeedbackMessage = "Program has been added";
                    return RedirectToPage(new { programId = programId });
                }
                catch (Exception ex)
                {
                    ErrorMessage = GetInnerException(ex).Message;
                    SchoolList = _programservices.School_List();
                    return Page();
                }
            }
            else
            {
                return Page();
            }
        }
        public IActionResult OnPostUpdate()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsaffected = _programservices.Program_Update(SchoolProgramInfo);
                    if (rowsaffected > 0)
                    {
                        FeedbackMessage = "Program has been updated";

                    }
                    else
                    {
                        FeedbackMessage = "Program has been not been updated. Territory does not appear to be on file. Refresh your search.";

                    }
                    return RedirectToPage(new { programId = programId });
                }
                catch (Exception ex)
                {
                    ErrorMessage = GetInnerException(ex).Message;
                    SchoolList = _programservices.School_List();
                    return Page();
                }
            }
            else
            {
                return Page();
            }
        }

        public IActionResult OnPostRemove()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int rowsaffected = _programservices.Program_Delete(SchoolProgramInfo);
                    if (rowsaffected > 0)
                    {
                        FeedbackMessage = "Program has been removed";

                    }
                    else
                    {
                        FeedbackMessage = "Program has been not been removed. Program does not appear to be on file. Refresh your search.";

                    }
                    return RedirectToPage(new { programId = "" });
                }
                catch (Exception ex)
                {
                    ErrorMessage = GetInnerException(ex).Message;
                    SchoolList = _programservices.School_List();
                    return Page();
                }
            }
            else
            {
                return Page();
            }
        }

        public IActionResult OnPostClear()
        {

            return RedirectToPage(new { programId = "" });
        }

        public IActionResult OnPostBack()
        {
            return Redirect("/Query");
        }

        private Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;
            return ex;
        }
    }
}
