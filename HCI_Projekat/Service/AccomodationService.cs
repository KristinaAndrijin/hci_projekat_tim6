using HCI_Projekat.Model;
using HCI_Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppContext = HCI_Projekat.Repository.AppContext;

namespace HCI_Projekat.Service
{
    internal class AccomodationService
    {
        public static Accomodation? Add(string address, AccomodationType type, string name)
        {
            Accomodation newAccomodation = new Accomodation()
            {
                Address = address,
                Type = type,
                Name = name
            };
            using (var context = new AppContext())
            {
                context.Accomodations.Add(newAccomodation);
                context.SaveChanges();
            }
            return newAccomodation;
        }

        public static Accomodation? Edit(int id, string address, AccomodationType type, string name) {
            using (var context = new AppContext())
            {
                var acc = context.Accomodations.Find(id);
                if (acc != null)
                {
                    acc.Address = address;
                    acc.Type = type;
                    acc.Name = name;
                    context.SaveChanges();
                    return acc;
                }
                return null;
            }
        }

        public static List<Accomodation> GetAllAccomodations()
        {
            using (var context = new AppContext())
            {
                return context.Accomodations.ToList();
            }
        }

        public static Accomodation? FindById(int id)
        {
            using (var context = new AppContext())
            {
                var acc = context.Accomodations.Find(id);
                if (acc != null)
                {
                    return acc;
                }
                return null;
            }
        }
    }
}
