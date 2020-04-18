using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System.Collections.Generic;

namespace Project.Providers
{
    public class AccessProvider : IDatabaseProvider
    {
        public Bank GetBank(int id)
        {
            return new Bank()
            {
                Id = id,
                Name = "Банк",
                CorrespondentAccount = "111",
                Bik = "1111"
            };
        }

        public List<Bank> GetBanks()
        {
            return new List<Bank>()
            {
                new Bank()
                {
                    Id = 1,
                    Name = "Банк",
                    CorrespondentAccount = "111",
                    Bik  = "1111"
                }
            };
        }

        public Material GetMaterial(int id)
        {
            return new Material()
            {
                Id = id,
                DeliveriesStopped = true,
                Description = "Описание",
                Name = "Наименование",
                Supplier = new Supplier()
                {
                    Id = 1,
                    OrganizationName = "Поставщик №1"
                },
                Сategory = new MaterialСategory()
                {
                    Id = 1,
                    Name = "Категория №1"
                }
            };
        }

        public List<Material> GetMaterials()
        {
            return new List<Material>()
            {
                new Material()
                {
                Id = 1,
                DeliveriesStopped = true,
                Description = "Описание",
                Name = "Наименование",
                Supplier = new Supplier()
                {
                    Id = 1,
                    OrganizationName = "Поставщик №1"
                },
                Сategory = new MaterialСategory()
                {
                    Id = 1,
                    Name = "Категория №1"
                }
            }
        };
        }

        public List<MaterialСategory> GetMaterialСategories()
        {
            return new List<MaterialСategory>()
            {
                new MaterialСategory()
                {
                    Id = 1,
                    Name = "Категория"
                }
            };
        }

        public MaterialСategory GetMaterialСategory(int id)
        {
            return new MaterialСategory()
            {
                Id = 1,
                Name = "Категория"
            };
        }

        public Supplier GetSupplier(int id)
        {
            return new Supplier()
            {
                Id = id,
                OrganizationName = "Поставщик №1",
                INN = "12345678",
                KPP = "1111111"
            };
        }

        public List<Supplier> GetSuppliers()
        {
            return new List<Supplier>()
            {
                new Supplier()
                {
                    Id = 1,
                    OrganizationName = "Поставщик №1",
                    INN = "12345678",
                    KPP = "1111111"
                }
            };
        }

        public void SaveBank(Bank bank)
        {
            // TODO: Реализовать сохранение
        }

        public void SaveMaterial(Material material)
        {
            // TODO: Реализовать сохранение
        }

        public void SaveMaterialСategory(MaterialСategory materialСategory)
        {
            // TODO: Реализовать сохранение
        }

        public void SaveSupplier(Supplier supplier)
        {
            // TODO: Реализовать сохранение
        }
    }
}
