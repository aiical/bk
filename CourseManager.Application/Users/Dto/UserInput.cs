using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using PagedInputDto.Dto;

namespace CourseManager.Users.Dto
{
    public class UserInput : PagedSortedAndFilteredInputDto
    {
       
        public string UserName { get; set; }

       
        public string Name { get; set; }

       
        public bool IsActive { get; set; }
    }
}