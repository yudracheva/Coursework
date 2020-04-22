using Project.Interfaces;
using Project.Models.Documents;
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
                Category = new MaterialCategory()
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
                Category = new MaterialCategory()
                {
                    Id = 1,
                    Name = "Категория №1"
                }
            }
        };
        }

        public List<MaterialCategory> GetMaterialСategories()
        {
            return new List<MaterialCategory>()
            {
                new MaterialCategory()
                {
                    Id = 1,
                    Name = "Категория"
                }
            };
        }

        public MaterialCategory GetMaterialCategory(int id)
        {
            return new MaterialCategory()
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

        public void RemoveBank(int id)
        {
            throw new System.NotImplementedException();
        }

        public void SaveBank(Bank bank)
        {
            // TODO: Реализовать сохранение
        }

        public void SaveMaterial(Material material)
        {
            // TODO: Реализовать сохранение
        }

        public void SaveMaterialCategory(MaterialCategory materialCategory)
        {
            // TODO: Реализовать сохранение
        }

        public void SaveSupplier(Supplier supplier)
        {
            // TODO: Реализовать сохранение
        }

        public void UpdateBank(Bank bank)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveSupplier(int id)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveMaterial(int id)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveMaterialCategory(int id)
        {
            throw new System.NotImplementedException();
        }

        public void SaveActOfReceipt(ActOfReceipt document)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveActOfReceipt(int id)
        {
            throw new System.NotImplementedException();
        }

        public ActOfReceipt GetActOfReceipt(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<ActOfReceipt> GetActsOfReceipt()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveCorrectionOfBalanceMaterials(int id)
        {
            throw new System.NotImplementedException();
        }

        public void RemovePaymentRequest(int id)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveOrdersToSuppliers(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<OrdersToSuppliers> GetOrdersToSuppliers()
        {
            throw new System.NotImplementedException();
        }

        public List<CorrectionOfBalanceMaterials> GetCorrectionsOfBalanceMaterials()
        {
            throw new System.NotImplementedException();
        }

        public List<PaymentRequest> GetPaymentsRequests()
        {
            throw new System.NotImplementedException();
        }

        public PaymentRequest GetPaymentRequest(int id)
        {
            throw new System.NotImplementedException();
        }

        public OrdersToSuppliers GetOrderToSupplier(int id)
        {
            throw new System.NotImplementedException();
        }

        public OrdersToSuppliers GetOrdersToSuppliers(int id)
        {
            throw new System.NotImplementedException();
        }

        public CorrectionOfBalanceMaterials GetCorrectionOfBalanceMaterials(object id)
        {
            throw new System.NotImplementedException();
        }

        public void SaveCorrectionOfBalanceMaterials(CorrectionOfBalanceMaterials document)
        {
            throw new System.NotImplementedException();
        }

        public void SavePaymentRequest(PaymentRequest document)
        {
            throw new System.NotImplementedException();
        }

        public void SaveOrderToSupplier(OrdersToSuppliers document)
        {
            throw new System.NotImplementedException();
        }
    }
}
