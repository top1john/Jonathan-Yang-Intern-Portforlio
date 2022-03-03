
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using starTEDSystem.DAL;
using starTEDSystem.Entities;

namespace starTEDSystem.BLL
{
    public class ProgramServices
    {
        #region Context variable & constructor
        private readonly StarTEDContext _context;

        internal ProgramServices(StarTEDContext context)
        {
            _context = context;
        }
        #endregion


        public List<SchoolProgram> Program_GetByPartialDescription(string partialdescription, int pagenumber,
                                                      int pagesize,
                                                      out int totalcount)
        {
            IEnumerable<SchoolProgram> info = _context.Programs
                                               .Where(x => x.ProgramName.Contains(partialdescription));
           
            totalcount = info.Count();

            int skipRows = (pagenumber - 1) * pagesize;

            return info.Skip(skipRows).Take(pagesize).ToList();
        }

        public List<School> School_List()
        {
            IEnumerable<School> info = _context.Schools
        .OrderBy(x => x.SchoolName);

            return info.ToList();
        }


        
        public SchoolProgram Program_GetByProgramID(int programid)
        {
            return  _context.Programs
        .Where(x => x.ProgramId == programid)
        .FirstOrDefault();
            
        }

        #region Add, Update and Delete (Deactivate)
        public void Program_Add(SchoolProgram item)
        {

            var exist = _context.Programs.Find(item.ProgramId);
            if (exist != null)
            {
                throw new Exception("Program ID is already in use. Choose a different ID value");
            }

            _context.Programs.Add(item);

            _context.SaveChanges();

        }

        public int Program_Update(SchoolProgram item)
        {
            EntityEntry<SchoolProgram> updating = _context.Entry(item);
            updating.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            return _context.SaveChanges();
        }

        public int Program_Delete(SchoolProgram item)
        {

            EntityEntry<SchoolProgram> deleting = _context.Entry(item);
            deleting.State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

            return _context.SaveChanges();
        }

        #endregion


        /*
        public List<SchoolProgram> Program_Count()
        {
            IEnumerable<ProgramCourse> info = _context.ProgramCourses
            .Where(x => x.ProgramID == item.ProgramID);

        }
        */
        






    }
}
