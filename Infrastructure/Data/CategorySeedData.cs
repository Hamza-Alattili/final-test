using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class CategorySeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "طب عام", Code = CategoryEnum.GeneralPractice },
                new Category { Id = 2, Name = "باطنية", Code = CategoryEnum.InternalMedicine },
                new Category { Id = 3, Name = "جراحة", Code = CategoryEnum.Surgery },
                new Category { Id = 4, Name = "أطفال", Code = CategoryEnum.Pediatrics },
                new Category { Id = 5, Name = "نسائية وتوليد", Code = CategoryEnum.ObstetricsGynecology },
                new Category { Id = 6, Name = "عظام", Code = CategoryEnum.Orthopedics },
                new Category { Id = 7, Name = "عيون", Code = CategoryEnum.Ophthalmology },
                new Category { Id = 8, Name = "جلدية وتجميل", Code = CategoryEnum.Dermatology },
                new Category { Id = 9, Name = "أسنان", Code = CategoryEnum.Dentistry },
                new Category { Id = 10, Name = "نفسية وأعصاب", Code = CategoryEnum.Psychiatry },
                new Category { Id = 11, Name = "أذن وأنف وحنجرة", Code = CategoryEnum.ENT },
                new Category { Id = 12, Name = "طب بيطري", Code = CategoryEnum.Veterinary },
                new Category { Id = 13, Name = "أشعة", Code = CategoryEnum.Radiology },
                new Category { Id = 14, Name = "تغذية", Code = CategoryEnum.Nutrition }
            );
        }
    }
}
