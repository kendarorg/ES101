using Bus;
using Es.Lib;
using EsInMemory.Lib;
using InMemory.Bus;
using InMemory.Crud;
using Inventory.Domain;
using Inventory.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CQRSGui
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AreaRegistration.RegisterAllAreas();

            IEsFactory esFactory = EsFactory.Factory;
            IBus bus = new InMemoryBusRunning();

            IEventStore storage = new InMemoryEventStore(bus, esFactory);
            var commands = new InventoryCommandHandlers(bus, storage);
            ServiceLocator.InventoryItemDetailViewRepo = new InMemoryRepository<InventoryItemDetailsDto>();
            var detail = new InventoryItemDetails(bus, ServiceLocator.InventoryItemDetailViewRepo);

            ServiceLocator.InventoryItemListRepo = new InMemoryRepository<InventoryItemListDto>();
            var list = new InventoryListView(bus, ServiceLocator.InventoryItemListRepo);
            ServiceLocator.Bus = bus;
        }
    }
}
