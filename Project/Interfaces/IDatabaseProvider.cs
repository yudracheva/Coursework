using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using System.Collections.Generic;

namespace Project.Interfaces
{
    /// <summary>
    /// Интерфейс для доступа в БД
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Возвращает информацию о материале
        /// </summary>
        /// <param name="id">Идентификатор материала</param>
        /// <returns>Материал</returns>
        Material GetMaterial(int id);

        /// <summary>
        /// Возвращает список материалов
        /// </summary>
        /// <returns>Список всех материалов</returns>
        List<Material> GetMaterials();
        
        void RemoveCorrectionOfBalanceMaterials(int id);
        
        void RemovePaymentRequest(int id);
        
        void RemoveOrdersToSuppliers(int id);
        
        PaymentRequest GetPaymentRequest(int id);
        
        OrdersToSuppliers GetOrderToSupplier(int id);

        CorrectionOfBalanceMaterials GetCorrectionOfBalanceMaterials(int id);

        List<OrdersToSuppliers> GetOrdersToSuppliers();

        /// <summary>
        /// Возвращает информацию по выбранному поставщику
        /// </summary>
        /// <param name="id">ИДентификатор поставщика</param>
        /// <returns>Поставщик</returns>
        Supplier GetSupplier(int id);

        List<CorrectionOfBalanceMaterials> GetCorrectionsOfBalanceMaterials();
       
        void SaveCorrectionOfBalanceMaterials(CorrectionOfBalanceMaterials document);
        
        List<PaymentRequest> GetPaymentsRequests();
        
        void SavePaymentRequest(PaymentRequest document);

        /// <summary>
        /// Возвращает список поставщиков
        /// </summary>
        /// <returns>Список поставщиков</returns>
        List<Supplier> GetSuppliers();
        
        void SaveOrderToSupplier(OrdersToSuppliers document);

        /// <summary>
        /// Возвращает категорию материалов
        /// </summary>
        /// <param name="id">Ижентификатор категории</param>
        /// <returns>Категорий материалов</returns>
        MaterialCategory GetMaterialCategory(int id);

        /// <summary>
        /// Возвратает запрощенный банк
        /// </summary>
        /// <param name="id">Идентификатор банка</param>
        /// <returns>Банк</returns>
        Bank GetBank(int id);

        /// <summary>
        /// Возвращает список категорий
        /// </summary>
        /// <returns>Список категорий</returns>
        List<MaterialCategory> GetMaterialСategories();

        /// <summary>
        /// Возвращает список банков
        /// </summary>
        /// <returns>Список банков</returns>
        List<Bank> GetBanks();

        /// <summary>
        /// Сохраняет или обновляет поставщика
        /// </summary>
        /// <param name="supplier">Поставщик</param>
        void SaveSupplier(Supplier supplier);

        /// <summary>
        /// Удаление поставщика
        /// </summary>
        /// <param name="id">ИДентификатор поставщика</param>
        void RemoveSupplier(int id);

        /// <summary>
        /// Сохраняет банк
        /// </summary>
        /// <param name="bank">Банк</param>
        void SaveBank(Bank bank);

        /// <summary>
        /// Удаление банка
        /// </summary>
        /// <param name="id">ИДентификатор банка</param>
        void RemoveBank(int id);

        /// <summary>
        /// Сохраняет или обновляет материал
        /// </summary>
        /// <param name="material">Материал</param>
        void SaveMaterial(Material material);

        /// <summary>
        /// Удаление материала
        /// </summary>
        /// <param name="id">ИДентификатор материала</param>
        void RemoveMaterial(int id);

        /// <summary>
        /// Сохраняет или обновляет категорию материала
        /// </summary>
        /// <param name="materialCategory">Категория материала</param>
        void SaveMaterialCategory(MaterialCategory materialCategory);

        /// <summary>
        /// Удаление категории материала
        /// </summary>
        /// <param name="id">ИДентификатор категории материала</param>
        void RemoveMaterialCategory(int id);

        /// <summary>
        /// Сохранение акта
        /// </summary>
        /// <param name="document">Акт</param>
        void SaveActOfReceipt(ActOfReceipt document);

        /// <summary>
        /// Сохранение акта
        /// </summary>
        /// <param name="document">Акт</param>
        void RemoveActOfReceipt(int id);

        /// <summary>
        /// Возвращает акт
        /// </summary>
        /// <param name="id">Номер акта</param>
        /// <returns>Документ</returns>
        ActOfReceipt GetActOfReceipt(int id);

        /// <summary>
        /// Возвращает список актов
        /// </summary>
        /// <returns>Список документа</returns>
        List<ActOfReceipt> GetActsOfReceipt();
    }
}
