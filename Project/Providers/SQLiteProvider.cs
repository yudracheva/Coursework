using Project.Interfaces;
using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using Project.Models.Reports;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Project.Providers
{
    public class SQLiteProvider : IDatabaseProvider
    {
        private readonly SettingsProvider _settingsProvider;
        private const string DATE_STRING = "MM/dd/yyyy HH:mm:ss";

        public SQLiteProvider(SettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentException(nameof(settingsProvider));
        }

        public Bank GetBank(int id)
        {
            var sql = @"select ID, 
                               NAME, 
                               BIK, 
                               CORRESPONDENT_ACCOUNT 
                          from BANK
                         where ID = @Id";
            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    using (var dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.Read())
                        {
                            var result = new Bank()
                            {
                                Id = dbReader.GetInt("ID"),
                                Bik = dbReader.GetString("BIK"),
                                CorrespondentAccount = dbReader.GetString("CORRESPONDENT_ACCOUNT"),
                                Name = dbReader.GetString("NAME"),
                            };

                            return result;
                        }
                    }
                }
            }

            return null;
        }

        public List<Bank> GetBanks()
        {
            var result = new List<Bank>();

            var sql = @"select ID, 
                               NAME, 
                               BIK, 
                               CORRESPONDENT_ACCOUNT 
                          from BANK";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var bank = new Bank()
                    {
                        Id = dbReader.GetInt("ID"),
                        Bik = dbReader.GetString("BIK"),
                        CorrespondentAccount = dbReader.GetString("CORRESPONDENT_ACCOUNT"),
                        Name = dbReader.GetString("NAME"),
                    };

                    result.Add(bank);
                }
            }

            return result;
        }

        public void RemoveBank(int id)
        {
            var sql = @"delete from BANK where ID = @Id";

            using var con = new SQLiteConnection(_settingsProvider.ConnectionString);
            con.Open();

            using var cmd = new SQLiteCommand(sql, con);
            cmd.AddParameter("@Id", id);

            cmd.ExecuteNonQuery();
        }

        public void SaveBank(Bank bank)
        {
            var id = bank.Id;
            if (id == 0)
                id = GetGenNumber_Bank();

            var sql = @"insert or replace into BANK (ID, NAME, BIK, CORRESPONDENT_ACCOUNT)
                                             values (@Id, @Name, @Bik, @Account)";

            using var con = new SQLiteConnection(_settingsProvider.ConnectionString);
            con.Open();

            using var cmd = new SQLiteCommand(sql, con);
            cmd.AddParameter("@Id", id);
            cmd.AddParameter("@Name", bank.Name);
            cmd.AddParameter("@Bik", bank.Bik);
            cmd.AddParameter("@Account", bank.CorrespondentAccount);

            cmd.ExecuteNonQuery();
        }

        public Material GetMaterial(int id)
        {
            var sql = @"select m.ID, 
	                           m.NAME, 
	                           m.DESCRIPTION,
	                           m.SUPPLIER,
	                           m.DELIVERIES_STOPED,
	                           m.Category,
	                           s.NAME as SUPPLIERNAME,
	                           c.NAME as CATEGORYNAME
                          from MATERIALS m
                     left join SUPPLIERS s
                            on m.SUPPLIER = s.ID
                     left join MATERIALS_CATEGORIES c
                            on m.Category = c.ID
                         where m.ID = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                cmd.AddParameter("@Id", id);

                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var material = new Material()
                    {
                        Id = dbReader.GetInt("ID"),
                        Name = dbReader.GetString("NAME"),
                        DeliveriesStopped = dbReader.GetInt("DELIVERIES_STOPED") == 1,
                        Description = dbReader.GetString("DESCRIPTION"),
                    };

                    material.Supplier = new Supplier()
                    {
                        Id = dbReader.GetInt("SUPPLIER"),
                        OrganizationName = dbReader.GetString("SUPPLIERNAME")
                    };

                    material.Category = new MaterialCategory()
                    {
                        Id = dbReader.GetInt("Category"),
                        Name = dbReader.GetString("CATEGORYNAME")
                    };

                    return material;
                }
            }

            return null;
        }

        public List<Material> GetMaterials()
        {
            var result = new List<Material>();

            var sql = @"select m.ID, 
	                           m.NAME, 
	                           m.DESCRIPTION,
	                           m.SUPPLIER,
	                           m.DELIVERIES_STOPED,
	                           m.Category,
	                           s.NAME as SUPPLIERNAME,
	                           c.NAME as CATEGORYNAME
                          from MATERIALS m
                     left join SUPPLIERS s
                            on m.SUPPLIER = s.ID
                     left join MATERIALS_CATEGORIES c
                            on m.Category = c.ID";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var material = new Material()
                    {
                        Id = dbReader.GetInt("ID"),
                        Name = dbReader.GetString("NAME"),
                        DeliveriesStopped = dbReader.GetInt("DELIVERIES_STOPED") == 1,
                        Description = dbReader.GetString("DESCRIPTION"),
                    };

                    material.Supplier = new Supplier()
                    {
                        Id = dbReader.GetInt("SUPPLIER"),
                        OrganizationName = dbReader.GetString("SUPPLIERNAME")
                    };

                    material.Category = new MaterialCategory()
                    {
                        Id = dbReader.GetInt("Category"),
                        Name = dbReader.GetString("CATEGORYNAME")
                    };

                    result.Add(material);
                }
            }

            return result;
        }

        public List<MaterialCategory> GetMaterialСategories()
        {
            var result = new List<MaterialCategory>();

            var sql = @"select ID, 
	                           NAME
                          from MATERIALS_CATEGORIES";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var material = new MaterialCategory()
                    {
                        Id = dbReader.GetInt("ID"),
                        Name = dbReader.GetString("NAME"),
                    };

                    result.Add(material);
                }
            }

            return result;
        }

        public MaterialCategory GetMaterialCategory(int id)
        {
            var sql = @"select ID, 
	                           NAME
                          from MATERIALS_CATEGORIES
                         where Id = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                cmd.AddParameter("@Id", id);

                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var material = new MaterialCategory()
                    {
                        Id = dbReader.GetInt("ID"),
                        Name = dbReader.GetString("NAME"),
                    };

                    return material;
                }
            }

            return null;
        }

        public Supplier GetSupplier(int id)
        {
            var sql = @"select m.ID, 
	                           m.NAME,
	                           m.EMAIL,
	                           m.INN,
	                           m.KPP,
	                           m.PAYMENT_ACCOUNT,
	                           m.BANK,
                               b.NAME as BANKNAME
                          from SUPPLIERS m
                     left join BANK b
                            on m.BANK = b.ID
                         where m.ID = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    using (var dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.Read())
                        {
                            var supplier = new Supplier()
                            {
                                Id = dbReader.GetInt("ID"),
                                OrganizationName = dbReader.GetString("NAME"),
                                Email = dbReader.GetString("Email"),
                                INN = dbReader.GetString("INN"),
                                KPP = dbReader.GetString("KPP"),
                                BankAccount = dbReader.GetString("PAYMENT_ACCOUNT"),
                            };

                            var idBank = dbReader.GetInt("BANK");

                            if (idBank != 0)
                            {
                                supplier.Bank = new Bank()
                                {
                                    Id = dbReader.GetInt("BANK"),
                                    Name = dbReader.GetString("BANKNAME"),
                                };
                            }

                            return supplier;
                        }
                    }
                }
            }

            return null;
        }

        public List<Supplier> GetSuppliers()
        {
            var result = new List<Supplier>();

            var sql = @"select m.ID, 
	                           m.NAME,
	                           m.Email,
	                           m.INN,
	                           m.KPP,
	                           m.PAYMENT_ACCOUNT,
	                           m.BANK,
                               b.NAME as BANKNAME
                          from SUPPLIERS m
                     left join BANK b
                            on m.BANK = b.ID";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using (var dbReader = cmd.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        var supplier = new Supplier()
                        {
                            Id = dbReader.GetInt("ID"),
                            OrganizationName = dbReader.GetString("NAME"),
                            Email = dbReader.GetString("Email"),
                            INN = dbReader.GetString("INN"),
                            KPP = dbReader.GetString("KPP"),
                            BankAccount = dbReader.GetString("PAYMENT_ACCOUNT"),
                        };

                        var idBank = dbReader.GetInt("BANK");

                        if (idBank != 0)
                        {
                            supplier.Bank = new Bank()
                            {
                                Id = dbReader.GetInt("BANK"),
                                Name = dbReader.GetString("BANKNAME"),
                            };
                        }

                        result.Add(supplier);
                    }
                }
            }

            return result;
        }

        public void SaveMaterial(Material material)
        {
            var id = material.Id;
            if (id == 0)
                id = GetGenNumber_Material();

            var sql = @"insert or replace into MATERIALS (ID, NAME, DESCRIPTION, SUPPLIER, DELIVERIES_STOPED, Category)
                                  values (@Id, @Name, @Description, @Supplier, @DeliveriesStopped, @Category)";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);
                    cmd.AddParameter("@Name", material.Name);
                    cmd.AddParameter("@Description", material.Description);
                    cmd.AddParameter("@Supplier", material.Supplier?.Id ?? 0);
                    cmd.AddParameter("@DeliveriesStopped", material.DeliveriesStopped);
                    cmd.AddParameter("@Category", material.Category?.Id ?? 0);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveMaterialCategory(MaterialCategory materialCategory)
        {
            var id = materialCategory.Id;
            if (id == 0)
                id = GetGenNumber_MaterialCategory();

            var sql = @"insert or replace into MATERIALS_CATEGORIES (ID, NAME)
                                                             values (@Id, @Name)";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);
                    cmd.AddParameter("@Name", materialCategory.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveSupplier(Supplier supplier)
        {
            var id = supplier.Id;
            if (id == 0)
                id = GetGenNumber_Supplier();

            var sql = @"insert or replace into SUPPLIERS (ID, NAME, Email, INN, KPP, PAYMENT_ACCOUNT, BANK)
                                                  values (@Id, @Name, @Email, @INN, @KPP, @Account, @Bank)";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);
                    cmd.AddParameter("@Name", supplier.OrganizationName);
                    cmd.AddParameter("@Email", supplier.Email);
                    cmd.AddParameter("@INN", supplier.INN);
                    cmd.AddParameter("@KPP", supplier.KPP);
                    cmd.AddParameter("@Account", supplier.BankAccount);
                    cmd.AddParameter("@Bank", supplier.Bank?.Id ?? 0);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveSupplier(int id)
        {
            var sql = @"delete from SUPPLIERS where ID = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveMaterial(int id)
        {
            var sql = @"delete from MATERIALS where ID = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveMaterialCategory(int id)
        {
            var sql = @"delete from MATERIALS_CATEGORIES where ID = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveActOfReceipt(ActOfReceipt document)
        {
            var id = document.Number;
            if (id == 0)
                id = GetGenNumber_ActOfReceipt();

            using var con = new SQLiteConnection(_settingsProvider.ConnectionString);

            // Очистим все строки, а потом добавим
            var sql = @"delete from RECEIPT_OF_MATERIALS_LINES where DOCUMENT_NUMBER = @Id";

            con.Open();

            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.AddParameter("@Id", id);

                cmd.ExecuteNonQuery();
            }

            sql = @"insert or replace into RECEIPT_OF_MATERIALS (Number, Date, Supplier)
                                                         values (@DocumentNumber, @DocumentDate, @Supplier)";

            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.AddParameter("@DocumentNumber", id);
                cmd.AddParameter("@DocumentDate", document.CreatedDate.ToString(DATE_STRING));
                cmd.AddParameter("@Supplier", document.Supplier?.Id ?? 0);

                cmd.ExecuteNonQuery();
            }

            sql = @"insert or replace into RECEIPT_OF_MATERIALS_LINES (DOCUMENT_NUMBER, NUMBER, MATERIAL, COUNT, PRICE, SUM)
                                                              values (@DocumentNumber, @Number,  @Material, @Count, @Price, @Sum)";

            foreach (var item in document.Materials)
            {
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@DocumentNumber", id);
                    cmd.AddParameter("@Number", item.Number);
                    cmd.AddParameter("@Material", item?.Material?.Id ?? 0);
                    cmd.AddParameter("@Count", item.Count);
                    cmd.AddParameter("@Price", item.Price);
                    cmd.AddParameter("@Sum", item.Sum);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveActOfReceipt(int id)
        {
            var sql = @"delete from RECEIPT_OF_MATERIALS_LINES where DOCUMENT_NUMBER = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }

            sql = @"delete from RECEIPT_OF_MATERIALS where NUMBER = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public ActOfReceipt GetActOfReceipt(int id)
        {
            var sql = @"select NUMBER,
                               DATE,
                               SUPPLIER 
                          from RECEIPT_OF_MATERIALS";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                if (dbReader.Read())
                {
                    var actOfReceipt = new ActOfReceipt()
                    {
                        Number = dbReader.GetInt("NUMBER"),
                        CreatedDate = dbReader.GetDateTime("DATE")
                    };

                    var supplier = dbReader.GetInt("SUPPLIER");
                    if (supplier != 0)
                        actOfReceipt.Supplier = GetSupplier(supplier);

                    actOfReceipt.Materials = GetActOfReceiptMaterials(id);

                    return actOfReceipt;
                }
            }

            return null;
        }

        private List<LineOfMaterials> GetActOfReceiptMaterials(int id)
        {
            var lines = new List<LineOfMaterials>();

            var sql = @"select NUMBER,
                               PRICE,
                               COUNT,
                               SUM,
                               MATERIAL
                          from RECEIPT_OF_MATERIALS_LINES
                         where DOCUMENT_NUMBER = @DocumentNumber";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                cmd.AddParameter("@DocumentNumber", id);

                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var line = new LineOfMaterials()
                    {
                        Number = dbReader.GetInt("NUMBER"),
                        Price = dbReader.GetDecimal("PRICE"),
                        Sum = dbReader.GetDecimal("SUM"),
                        Count = dbReader.GetInt("COUNT"),
                        SelectedMaterial = dbReader.GetInt("MATERIAL")
                    };

                    if (line.SelectedMaterial != 0)
                        line.Material = GetMaterial(line.SelectedMaterial);

                    lines.Add(line);
                }
            }

            return lines;
        }

        public List<ActOfReceipt> GetActsOfReceipt()
        {
            var result = new List<ActOfReceipt>();

            var sql = @"select NUMBER,
                               DATE,
                               SUPPLIER 
                          from RECEIPT_OF_MATERIALS";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var actOfReceipt = new ActOfReceipt()
                    {
                        Number = dbReader.GetInt("NUMBER"),
                        CreatedDate = dbReader.GetDateTime("DATE")
                    };

                    var supplier = dbReader.GetInt("SUPPLIER");
                    if (supplier != 0)
                        actOfReceipt.Supplier = GetSupplier(supplier);

                    result.Add(actOfReceipt);
                }
            }

            return result;
        }

        public void RemoveCorrectionOfBalanceMaterials(int id)
        {
            var sql = @"delete from CORRECTION_OF_BALANCES_LINES where DOCUMENT_NUMBER = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }

            sql = @"delete from CORRECTION_OF_BALANCES where NUMBER = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemovePaymentRequest(int id)
        {
            var sql = @"delete from PAYMENT_REQUEST where NUMBER = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveOrdersToSuppliers(int id)
        {
            var sql = @"delete from ORDERS_TO_SUPPLIERS_LINES where DOCUMENT_NUMBER = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }

            sql = @"delete from ORDERS_TO_SUPPLIERS where NUMBER = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<OrdersToSuppliers> GetOrdersToSuppliers()
        {
            var result = new List<OrdersToSuppliers>();

            var sql = @"select NUMBER,
                               DATE,
                               SUPPLIER 
                          from ORDERS_TO_SUPPLIERS";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var ordersToSuppliers = new OrdersToSuppliers()
                    {
                        Number = dbReader.GetInt("NUMBER"),
                        CreatedDate = dbReader.GetDateTime("DATE")
                    };

                    var supplier = dbReader.GetInt("SUPPLIER");
                    if (supplier != 0)
                        ordersToSuppliers.Supplier = GetSupplier(supplier);

                    result.Add(ordersToSuppliers);
                }
            }

            return result;
        }

        public List<CorrectionOfBalanceMaterials> GetCorrectionsOfBalanceMaterials()
        {
            var result = new List<CorrectionOfBalanceMaterials>();

            var sql = @"select NUMBER,
                               DATE
                          from CORRECTION_OF_BALANCES";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var correctionOfBalanceMaterials = new CorrectionOfBalanceMaterials()
                    {
                        CreatedDate = dbReader.GetDateTimeOrMin("DATE"),
                        Number = dbReader.GetInt("NUMBER")
                    };

                    result.Add(correctionOfBalanceMaterials);
                }
            }

            return result;
        }

        public List<PaymentRequest> GetPaymentsRequests()
        {
            var result = new List<PaymentRequest>();

            var sql = @"select NUMBER,
                               DATE,
                               SUPPLIER,
                               SUM
                          from PAYMENT_REQUEST";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var material = new PaymentRequest()
                    {
                        CreatedDate = dbReader.GetDateTimeOrMin("DATE"),
                        Number = dbReader.GetInt("NUMBER"),
                        Sum = dbReader.GetDecimal("SUM")
                    };

                    var supplier = dbReader.GetInt("SUPPLIER");
                    if (supplier != 0)
                        material.Supplier = GetSupplier(supplier);

                    result.Add(material);
                }
            }

            return result;
        }

        private int GetGenNumber_Bank()
        {
            var sql = @"select max(ID) as ID from BANK";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.Read())
                        {
                            var result = dbReader.GetInt("ID");
                            return result + 1;
                        }
                    }
                }
            }

            return 1;
        }

        private int GetGenNumber_Supplier()
        {
            var sql = @"select max(ID) as ID from SUPPLIERS";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.Read())
                        {
                            var result = dbReader.GetInt("ID");
                            return result + 1;
                        }
                    }
                }
            }

            return 1;
        }

        private int GetGenNumber_MaterialCategory()
        {
            var sql = @"select max(ID) as ID from MATERIALS_CATEGORIES";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.Read())
                        {
                            var result = dbReader.GetInt("ID");
                            return result + 1;
                        }
                    }
                }
            }

            return 1;
        }

        private int GetGenNumber_Material()
        {
            var sql = @"select max(ID) as ID from MATERIALS";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.Read())
                        {
                            var result = dbReader.GetInt("ID");
                            return result + 1;
                        }
                    }
                }
            }

            return 1;
        }

        private int GetGenNumber_ActOfReceipt()
        {
            var sql = @"select max(NUMBER) as ID from RECEIPT_OF_MATERIALS";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.Read())
                        {
                            var result = dbReader.GetInt("ID");
                            return result + 1;
                        }
                    }
                }
            }

            return 1;
        }

        public PaymentRequest GetPaymentRequest(int id)
        {
            var sql = @"select NUMBER,
                               DATE,
                               SUPPLIER,
                               SUM
                          from PAYMENT_REQUEST
                         where NUMBER = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                cmd.AddParameter("@Id", id);
                using var dbReader = cmd.ExecuteReader();
                if (dbReader.Read())
                {
                    var paymentRequest = new PaymentRequest()
                    {
                        CreatedDate = dbReader.GetDateTimeOrMin("DATE"),
                        Number = dbReader.GetInt("NUMBER"),
                        Sum = dbReader.GetDecimal("SUM")
                    };

                    var supplier = dbReader.GetInt("SUPPLIER");
                    if (supplier != 0)
                        paymentRequest.Supplier = GetSupplier(supplier);

                    return paymentRequest;
                }
            }

            return null;
        }

        public OrdersToSuppliers GetOrderToSupplier(int id)
        {
            var sql = @"select NUMBER,
                               DATE,
                               SUPPLIER 
                          from ORDERS_TO_SUPPLIERS";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                if (dbReader.Read())
                {
                    var actOfReceipt = new OrdersToSuppliers()
                    {
                        Number = dbReader.GetInt("NUMBER"),
                        CreatedDate = dbReader.GetDateTime("DATE")
                    };

                    var supplier = dbReader.GetInt("SUPPLIER");
                    if (supplier != 0)
                        actOfReceipt.Supplier = GetSupplier(supplier);

                    actOfReceipt.Materials = GetOrderToSupplierMaterials(id);

                    return actOfReceipt;
                }
            }

            return null;
        }

        private List<LineOfMaterials> GetOrderToSupplierMaterials(int id)
        {
            var lines = new List<LineOfMaterials>();

            var sql = @"select NUMBER,
                               PRICE,
                               COUNT,
                               SUM,
                               MATERIAL
                          from ORDERS_TO_SUPPLIERS_LINES
                         where DOCUMENT_NUMBER = @DocumentNumber";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                cmd.AddParameter("@DocumentNumber", id);

                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var line = new LineOfMaterials()
                    {
                        Number = dbReader.GetInt("NUMBER"),
                        Price = dbReader.GetDecimal("PRICE"),
                        Sum = dbReader.GetDecimal("SUM"),
                        Count = dbReader.GetInt("COUNT"),
                        SelectedMaterial = dbReader.GetInt("MATERIAL")
                    };

                    if (line.SelectedMaterial != 0)
                        line.Material = GetMaterial(line.SelectedMaterial);

                    lines.Add(line);
                }
            }

            return lines;
        }

        public CorrectionOfBalanceMaterials GetCorrectionOfBalanceMaterials(int id)
        {
            var sql = @"select NUMBER,
                               DATE
                          from CORRECTION_OF_BALANCES
                         where NUMBER = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                cmd.AddParameter("@Id", id);
                using var dbReader = cmd.ExecuteReader();
                if (dbReader.Read())
                {
                    var correctionOfBalanceMaterials = new CorrectionOfBalanceMaterials()
                    {
                        CreatedDate = dbReader.GetDateTimeOrMin("DATE"),
                        Number = dbReader.GetInt("NUMBER")
                    };

                    correctionOfBalanceMaterials.Materials = GetCorrectionOfBalanceMaterialsMaterials(id);

                    return correctionOfBalanceMaterials;
                }
            }

            return null;
        }

        private List<LineOfMaterials> GetCorrectionOfBalanceMaterialsMaterials(int id)
        {
            var lines = new List<LineOfMaterials>();

            var sql = @"select NUMBER,
                               COUNT,
                               MATERIAL
                          from CORRECTION_OF_BALANCES_LINES
                         where DOCUMENT_NUMBER = @DocumentNumber";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                cmd.AddParameter("@DocumentNumber", id);

                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var line = new LineOfMaterials()
                    {
                        Number = dbReader.GetInt("NUMBER"),
                        Count = dbReader.GetInt("COUNT"),
                        SelectedMaterial = dbReader.GetInt("MATERIAL")
                    };

                    if (line.SelectedMaterial != 0)
                        line.Material = GetMaterial(line.SelectedMaterial);

                    lines.Add(line);
                }
            }

            return lines;
        }

        public void SaveCorrectionOfBalanceMaterials(CorrectionOfBalanceMaterials document)
        {
            var id = document.Number;
            if (id == 0)
                id = GetGenNumber_CorrectionOfBalanceMaterials();

            using var con = new SQLiteConnection(_settingsProvider.ConnectionString);

            // Очистим все строки, а потом добавим
            var sql = @"delete from CORRECTION_OF_BALANCES_LINES where DOCUMENT_NUMBER = @Id";

            con.Open();

            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.AddParameter("@Id", id);

                cmd.ExecuteNonQuery();
            }

            sql = @"insert or replace into CORRECTION_OF_BALANCES (Number, Date)
                                                         values (@DocumentNumber, @DocumentDate)";

            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.AddParameter("@DocumentNumber", id);
                cmd.AddParameter("@DocumentDate", document.CreatedDate.ToString(DATE_STRING));

                cmd.ExecuteNonQuery();
            }

            sql = @"insert or replace into CORRECTION_OF_BALANCES_LINES (DOCUMENT_NUMBER, NUMBER, MATERIAL, COUNT)
                                                              values (@DocumentNumber, @Number,  @Material, @Count)";

            foreach (var item in document.Materials)
            {
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@DocumentNumber", id);
                    cmd.AddParameter("@Number", item.Number);
                    cmd.AddParameter("@Material", item?.Material?.Id ?? 0);
                    cmd.AddParameter("@Count", item.Count);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private int GetGenNumber_CorrectionOfBalanceMaterials()
        {
            var sql = @"select max(Number) as ID from CORRECTION_OF_BALANCES";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.Read())
                        {
                            var result = dbReader.GetInt("ID");
                            return result + 1;
                        }
                    }
                }
            }

            return 1;
        }

        public void SavePaymentRequest(PaymentRequest document)
        {
            var id = document.Number;
            if (id == 0)
                id = GetGenNumber_PaymentRequest();

            var sql = @"insert or replace into PAYMENT_REQUEST (NUMBER, DATE, SUM, SUPPLIER)
                                  values (@Id, @DATE, @SUM, @Supplier)";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);
                    cmd.AddParameter("@DATE", document.CreatedDate.ToString(DATE_STRING));
                    cmd.AddParameter("@SUM", document.Sum);
                    cmd.AddParameter("@Supplier", document.Supplier?.Id ?? 0);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private int GetGenNumber_PaymentRequest()
        {
            var sql = @"select max(Number) as ID from PAYMENT_REQUEST";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
                        if (dbReader.Read())
                        {
                            var result = dbReader.GetInt("ID");
                            return result + 1;
                        }
                    }
                }
            }

            return 1;
        }

        public void SaveOrderToSupplier(OrdersToSuppliers document)
        {
            var id = document.Number;
            if (id == 0)
                id = GetGenNumber_CorrectionOfBalanceMaterials();

            using var con = new SQLiteConnection(_settingsProvider.ConnectionString);

            // Очистим все строки, а потом добавим
            var sql = @"delete from CORRECTION_OF_BALANCES_LINES where DOCUMENT_NUMBER = @Id";

            con.Open();

            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.AddParameter("@Id", id);

                cmd.ExecuteNonQuery();
            }

            sql = @"insert or replace into CORRECTION_OF_BALANCES (Number, Date)
                                                         values (@DocumentNumber, @DocumentDate)";

            using (var cmd = new SQLiteCommand(sql, con))
            {
                cmd.AddParameter("@DocumentNumber", id);
                cmd.AddParameter("@DocumentDate", document.CreatedDate.ToString(DATE_STRING));

                cmd.ExecuteNonQuery();
            }

            sql = @"insert or replace into CORRECTION_OF_BALANCES_LINES (DOCUMENT_NUMBER, NUMBER, MATERIAL, COUNT)
                                                              values (@DocumentNumber, @Number,  @Material, @Count)";

            foreach (var item in document.Materials)
            {
                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@DocumentNumber", id);
                    cmd.AddParameter("@Number", item.Number);
                    cmd.AddParameter("@Material", item?.Material?.Id ?? 0);
                    cmd.AddParameter("@Count", item.Count);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<ReportReceiptMaterialsBySupplier> GetReportReceiptMaterialsBySupplier()
        {
            var result = new List<ReportReceiptMaterialsBySupplier>();

            var sql = @"select r.NUMBER, 
                               r.DATE, 
                               r.SUPPLIER, 
                               l.MATERIAL, 
                               l.COUNT 
                          from RECEIPT_OF_MATERIALS as r 
                     left join RECEIPT_OF_MATERIALS_LINES as l 
                            on r.NUMBER = l.DOCUMENT_NUMBER";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using var cmd = new SQLiteCommand(sql, con);
                using var dbReader = cmd.ExecuteReader();
                while (dbReader.Read())
                {
                    var line = new ReportReceiptMaterialsBySupplier()
                    {
                        Count = dbReader.GetInt("COUNT"),
                        DocumentDate = dbReader.GetDateTime("DATE"),
                        DocumentNumber = dbReader.GetInt("NUMBER")
                    };

                    var supplierId = dbReader.GetInt("SUPPLIER");
                    if (supplierId != 0)
                        line.Supplier = GetSupplier(supplierId);

                    var materialId = dbReader.GetInt("MATERIAL");
                    if (materialId != 0)
                        line.Material = GetMaterial(materialId);

                    result.Add(line);
                }
            }

            return result;
        }
    }
}
