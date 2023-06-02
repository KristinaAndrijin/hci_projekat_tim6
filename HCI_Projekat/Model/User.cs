using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role Role { get; set; }

        //public User() { }

        //public User(string? name, string? lastname, string? email, string? password, Role role)
        //{
        //    Name = name;
        //    Lastname = lastname;
        //    Email = email;
        //    Password = password;
        //    Role = role;
        //}
    }
    public enum Role
    {
        User, Agent
    }
}
