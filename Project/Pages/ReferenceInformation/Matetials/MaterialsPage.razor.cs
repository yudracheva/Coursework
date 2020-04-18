﻿using Microsoft.AspNetCore.Components;
using Project.Interfaces;
using Project.Models.ReferenceInformation;
using System.Collections.Generic;

namespace Project.Pages.ReferenceInformation.Matetials
{
    public class MaterialsPageIndex : ComponentBase
    {
        [Inject]
        public IDatabaseProvider DatabaseProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<Material> materials;

        protected bool isLoad;

        protected override void OnInitialized()
        {
            Update();
        }

        protected void Edit(int id)
        {
            NavigationManager.NavigateTo($"/materials/{id:0}");
        }

        protected void Remove(int id)
        {
            DatabaseProvider.RemoveMaterial(id);
            Update();
        }

        public void Update()
        {
            isLoad = false;

            materials = DatabaseProvider.GetMaterials();

            isLoad = true;
        }
    }
}
