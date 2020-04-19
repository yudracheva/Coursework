using Project.Interfaces;
using Project.Models.ReferenceInformation;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Project.Providers
{
    public class SQLiteProvider : IDatabaseProvider
    {
        private readonly SettingsProvider _settingsProvider;

        public SQLiteProvider(SettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentException(nameof(settingsProvider));
        }

        public Bank GetBank(int id)
        {
            var sql = @"select ID, 
                               NAME, 
                               BIK, 
                               CORRESPONDENTACCOUNT 
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
                                CorrespondentAccount = dbReader.GetString("CORRESPONDENTACCOUNT"),
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
                               CORRESPONDENTACCOUNT 
                          from BANK";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
                        while (dbReader.Read())
                        {
                            var bank = new Bank()
                            {
                                Id = dbReader.GetInt("ID"),
                                Bik = dbReader.GetString("BIK"),
                                CorrespondentAccount = dbReader.GetString("CORRESPONDENTACCOUNT"),
                                Name = dbReader.GetString("NAME"),
                            };

                            result.Add(bank);
                        }
                    }
                }
            }

            return result;
        }

        public void RemoveBank(int id)
        {
            var sql = @"delete from BANK where ID = @Id";

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

        public void SaveBank(Bank bank)
        {
            var id = bank.Id;
            if (id == 0)
                id = GetGenNumber_Bank();

            var sql = @"insert or replace into BANK (ID, NAME, BIK, CORRESPONDENTACCOUNT)
                                  values (@Id, @Name, @Bik, @Account)";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);
                    cmd.AddParameter("@Name", bank.Name);
                    cmd.AddParameter("@Bik", bank.Bik);
                    cmd.AddParameter("@Account", bank.CorrespondentAccount);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Material GetMaterial(int id)
        {
            var sql = @"select m.ID, 
	                           m.NAME, 
	                           m.DESCRIPTION,
	                           m.SUPPLIER,
	                           m.DELIVERIESSTOPPED,
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

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    using (var dbReader = cmd.ExecuteReader())
                    {
                        while (dbReader.Read())
                        {
                            var material = new Material()
                            {
                                Id = dbReader.GetInt("ID"),
                                Name = dbReader.GetString("NAME"),
                                DeliveriesStopped = dbReader.GetInt("DELIVERIESSTOPPED") == 1,
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
	                           m.DELIVERIESSTOPPED,
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

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
                        while (dbReader.Read())
                        {
                            var material = new Material()
                            {
                                Id = dbReader.GetInt("ID"),
                                Name = dbReader.GetString("NAME"),
                                DeliveriesStopped = dbReader.GetInt("DELIVERIESSTOPPED") == 1,
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

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    using (var dbReader = cmd.ExecuteReader())
                    {
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
                }
            }

            return result;
        }

        public MaterialCategory GetMaterialCategory(int id)
        {
            var sql = @"select ID, 
	                           NAME
                          from MATERIALS
                         where Id = @Id";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.AddParameter("@Id", id);

                    using (var dbReader = cmd.ExecuteReader())
                    {
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
                }
            }

            return null;
        }

        public Supplier GetSupplier(int id)
        {
            var sql = @"select m.ID, 
	                           m.NAME,
	                           m.Email,
	                           m.INN,
	                           m.KPP,
	                           m.PAYMENTACCOUNT,
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
                                BankAccount = dbReader.GetString("PAYMENTACCOUNT"),
                            };

                            supplier.Bank = new Bank()
                            {
                                Id = dbReader.GetInt("BANK"),
                                Name = dbReader.GetString("BANKNAME"),
                            };

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
	                           m.PAYMENTACCOUNT,
	                           m.BANK,
                               b.NAME as BANKNAME
                          from SUPPLIERS m
                     left join BANK b
                            on m.BANK = b.ID";

            using (var con = new SQLiteConnection(_settingsProvider.ConnectionString))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
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
                                BankAccount = dbReader.GetString("PAYMENTACCOUNT"),
                            };

                            supplier.Bank = new Bank()
                            {
                                Id = dbReader.GetInt("BANK"),
                                Name = dbReader.GetString("BANKNAME"),
                            };

                            result.Add(supplier);
                        }
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

            var sql = @"insert or replace into MATERIALS (ID, NAME, DESCRIPTION, SUPPLIER, DELIVERIESSTOPPED, Category)
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

            var sql = @"insert or replace into SUPPLIERS (ID, NAME, Email, INN, KPP, PAYMENTACCOUNT, BANK)
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
    }
}
