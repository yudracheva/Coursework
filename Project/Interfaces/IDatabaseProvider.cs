using Project.Models.Documents;
using Project.Models.ReferenceInformation;
using Project.Models.Reports;
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
        List<Material> GetMaterials(int supplierId = 0, bool deliveriesStopped = false);
        
        /// <summary>
        /// Удялет документ "Корректировку остатков" по номеру
        /// </summary>
        /// <param name="id">Номер документа</param>
        void RemoveCorrectionOfBalanceMaterials(int id);

        /// <summary>
        /// Возвращает информацию для отчеты "Отчеты о поступлении материалов по поставщикам"
        /// </summary>
        /// <returns></returns>
        List<ReportReceiptMaterialsBySupplier> GetReportReceiptMaterialsBySupplier();
        
        /// <summary>
        /// Удаляет документ "Платежное требование" по номеру
        /// </summary>
        /// <param name="id">Номер</param>
        void RemovePaymentRequest(int id);
        
        /// <summary>
        /// Удаляет документ "Заказ поставщику" по номеру
        /// </summary>
        /// <param name="id">Номер документа</param>
        void RemoveOrdersToSuppliers(int id);
        
        /// <summary>
        /// Возвращает документ "Платежное требование" по номеру документа
        /// </summary>
        /// <param name="id">Номер документа</param>
        /// <returns></returns>
        PaymentRequest GetPaymentRequest(int id);
        
        /// <summary>
        /// Возвращает заказ поставщику по номеру
        /// </summary>
        /// <param name="id">Номер заказа поставщику</param>
        /// <returns></returns>
        OrdersToSuppliers GetOrderToSupplier(int id);

        /// <summary>
        /// Возвращает документ "Корректировка остатков" по идентификатору
        /// </summary>
        /// <param name="id">Номер документа</param>
        /// <returns></returns>
        CorrectionOfBalanceMaterials GetCorrectionOfBalanceMaterials(int id);

        /// <summary>
        /// Возвращает список документов "Заказы поставщику"
        /// </summary>
        /// <returns>Документов "Заказы поставщику"</returns>
        List<OrdersToSuppliers> GetOrdersToSuppliers();

        /// <summary>
        /// Возвращает информацию по отчеты "Материалы в пути"
        /// </summary>
        /// <returns></returns>
        List<ReportListsOfMaterialsOnTheWay> GetReportListsOfMaterialsOnTheWay();

        /// <summary>
        /// Возвращает информацию по выбранному поставщику
        /// </summary>
        /// <param name="id">ИДентификатор поставщика</param>
        /// <returns>Поставщик</returns>
        Supplier GetSupplier(int id);

        /// <summary>
        /// Возвращает список документов "Корректировка остатков"
        /// </summary>
        /// <returns></returns>
        List<CorrectionOfBalanceMaterials> GetCorrectionsOfBalanceMaterials();
       
        /// <summary>
        /// Сохраняет документ "Корректировка остатков"
        /// </summary>
        /// <param name="document">Документ</param>
        void SaveCorrectionOfBalanceMaterials(CorrectionOfBalanceMaterials document);
        
        /// <summary>
        /// Возвращает список документов "Платежное требование"
        /// </summary>
        /// <returns></returns>
        List<PaymentRequest> GetPaymentsRequests();
        
        /// <summary>
        /// Сохранение документа "Платежное требование"
        /// </summary>
        /// <param name="document">Документ</param>
        void SavePaymentRequest(PaymentRequest document);

        /// <summary>
        /// Возвращает список поставщиков
        /// </summary>
        /// <returns>Список поставщиков</returns>
        List<Supplier> GetSuppliers();
        
        /// <summary>
        /// Сохранение заказа поставщику
        /// </summary>
        /// <param name="document">Документ</param>
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
        /// Возвращает список доступного товара к приходу
        /// </summary>
        /// <param name="number">Номер заказа</param>
        /// <returns></returns>
        List<LineOfMaterials> GetAvailableOrderMaterial(int number);

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

        /// <summary>
        /// Возвращает список не закрытых заказов
        /// </summary>
        /// <returns></returns>
        List<OrdersToSuppliers> GetAvailableOrders();
    }
}
