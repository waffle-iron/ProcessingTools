﻿namespace ProcessingTools.Web.Areas.Data.Controllers
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AutoMapper;
    using ProcessingTools.Common.Exceptions;
    using ProcessingTools.Constants;
    using ProcessingTools.Contracts.Models.Geo;
    using ProcessingTools.Contracts.Services.Data.Geo;
    using ProcessingTools.Enumerations;
    using ProcessingTools.Models.ViewModels;
    using ProcessingTools.Web.Abstractions.Controllers;
    using ProcessingTools.Web.Areas.Data.Models.GeoNames;
    using ProcessingTools.Web.Constants;
    using Strings = ProcessingTools.Web.Areas.Data.Resources.GeoNames.Views_Strings;

    [Authorize]
    public class GeoNamesController : BaseMvcController
    {
        public const string AreaName = AreaNames.Data;
        public const string ControllerName = "GeoNames";
        public const string IndexActionName = RouteValues.IndexActionName;
        public const string DetailsActionName = nameof(GeoNamesController.Details);
        public const string CreateActionName = nameof(GeoNamesController.Create);
        public const string EditActionName = nameof(GeoNamesController.Edit);
        public const string DeleteActionName = nameof(GeoNamesController.Delete);

        private readonly IGeoNamesDataService service;
        private readonly IMapper mapper;

        public GeoNamesController(IGeoNamesDataService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));

            var mapperConfiguration = new MapperConfiguration(c =>
            {
                c.CreateMap<IGeoName, GeoNameViewModel>();
                c.CreateMap<GeoNameRequestModel, GeoNameViewModel>();
            });

            this.mapper = mapperConfiguration.CreateMapper();
        }

        [HttpGet, ActionName(IndexActionName)]
        public async Task<ActionResult> Index(int? p, int? n)
        {
            string returnUrl = this.Request[ContextKeys.ReturnUrl];
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            int currentPage = p ?? PagingConstants.DefaultPageNumber;
            if (currentPage < PagingConstants.MinimalPageNumber)
            {
                throw new InvalidPageNumberException();
            }

            int numberOfItemsPerPage = n ?? PagingConstants.DefaultLargeNumberOfItemsPerPage;
            if (numberOfItemsPerPage > PagingConstants.MaximalItemsPerPageAllowed)
            {
                throw new InvalidItemsPerPageException();
            }

            long numberOfItems = await this.service.SelectCountAsync(null);
            var data = await this.service.SelectAsync(null, currentPage * numberOfItemsPerPage, numberOfItemsPerPage, nameof(IGeoName.Name), SortOrder.Ascending);
            var items = data.Select(this.mapper.Map<GeoNameViewModel>).ToArray();

            var model = new ListWithPagingViewModel<GeoNameViewModel>(IndexActionName, numberOfItems, numberOfItemsPerPage, currentPage, items);
            var viewModel = new GeoNamesIndexPageViewModel
            {
                Model = model,
                PageTitle = Strings.IndexPageTitle
            };

            this.ViewData[ContextKeys.AreaName] = AreaName;
            this.ViewData[ContextKeys.ControllerName] = ControllerName;
            this.ViewData[ContextKeys.ActionName] = IndexActionName;
            this.ViewData[ContextKeys.BackActionName] = IndexActionName;
            return this.View(IndexActionName, viewModel);
        }

        [HttpGet, ActionName(DetailsActionName)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await this.service.GetByIdAsync(id);
            if (model == null)
            {
                return this.HttpNotFound();
            }

            var viewModel = new GeoNamePageViewModel
            {
                Model = this.mapper.Map<GeoNameViewModel>(model),
                PageTitle = Strings.DetailsPageTitle,
                ReturnUrl = this.Request[ContextKeys.ReturnUrl]
            };

            return this.View(DetailsActionName, viewModel);
        }

        [HttpGet, ActionName(CreateActionName)]
        public ActionResult Create()
        {
            var viewModel = new GeoNamePageViewModel
            {
                Model = new GeoNameViewModel { Id = -1 },
                PageTitle = Strings.CreatePageTitle,
                ReturnUrl = this.Request[ContextKeys.ReturnUrl]
            };

            return this.View(EditActionName, viewModel);
        }

        [HttpPost, ActionName(CreateActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name")] GeoNameRequestModel model)
        {
            string returnUrl = this.Request[ContextKeys.ReturnUrl];

            if (this.ModelState.IsValid)
            {
                await this.service.InsertAsync(model);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }

                return this.RedirectToAction(IndexActionName);
            }

            model.Id = -1;
            var viewModel = new GeoNamePageViewModel
            {
                Model = this.mapper.Map<GeoNameViewModel>(model),
                PageTitle = Strings.CreatePageTitle,
                ReturnUrl = returnUrl
            };

            return this.View(EditActionName, viewModel);
        }

        [HttpGet, ActionName(EditActionName)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await this.service.GetByIdAsync(id);
            if (model == null)
            {
                return this.HttpNotFound();
            }

            var viewModel = new GeoNamePageViewModel
            {
                Model = this.mapper.Map<GeoNameViewModel>(model),
                PageTitle = Strings.EditPageTitle,
                ReturnUrl = this.Request[ContextKeys.ReturnUrl]
            };

            return this.View(EditActionName, viewModel);
        }

        [HttpPost, ActionName(EditActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] GeoNameRequestModel model)
        {
            string returnUrl = this.Request[ContextKeys.ReturnUrl];

            if (this.ModelState.IsValid)
            {
                await this.service.UpdateAsync(model);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }

                return this.RedirectToAction(IndexActionName);
            }

            var viewModel = new GeoNamePageViewModel
            {
                Model = this.mapper.Map<GeoNameViewModel>(model),
                PageTitle = Strings.EditPageTitle,
                ReturnUrl = returnUrl
            };

            return this.View(EditActionName, viewModel);
        }

        [HttpGet, ActionName(DeleteActionName)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = await this.service.GetByIdAsync(id);
            if (model == null)
            {
                return this.HttpNotFound();
            }

            var viewModel = new GeoNamePageViewModel
            {
                Model = this.mapper.Map<GeoNameViewModel>(model),
                PageTitle = Strings.DeletePageTitle,
                ReturnUrl = this.Request[ContextKeys.ReturnUrl]
            };

            return this.View(DeleteActionName, viewModel);
        }

        [HttpPost, ActionName(DeleteActionName)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await this.service.DeleteAsync(ids: id);

            string returnUrl = this.Request[ContextKeys.ReturnUrl];
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(IndexActionName);
        }
    }
}
