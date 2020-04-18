using Project.Models;
using Project.Models.ReferenceInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// Возвращает информацию по выбранному поставщику
        /// </summary>
        /// <param name="id">ИДентификатор поставщика</param>
        /// <returns>Поставщик</returns>
        Supplier GetSupplier(int id);

        /// <summary>
        /// Возвращает список поставщиков
        /// </summary>
        /// <returns>Список поставщиков</returns>
        List<Supplier> GetSuppliers();

        /// <summary>
        /// Возвращает категорию материалов
        /// </summary>
        /// <param name="id">Ижентификатор категории</param>
        /// <returns>Категорий материалов</returns>
        MaterialСategory GetMaterialСategory(int id);
        
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
        List<MaterialСategory> GetMaterialСategories();

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
        /// Сохраняет или обновляет банк
        /// </summary>
        /// <param name="supplier">банк</param>
        void SaveBank(Bank bank);

        /// <summary>
        /// Сохраняет или обновляет материал
        /// </summary>
        /// <param name="material">Материал</param>
        void SaveMaterial(Material material);

        /// <summary>
        /// Сохраняет или обновляет категорию материала
        /// </summary>
        /// <param name="materialСategory">Категория материала</param>
        void SaveMaterialСategory(MaterialСategory materialСategory);
    }
}
