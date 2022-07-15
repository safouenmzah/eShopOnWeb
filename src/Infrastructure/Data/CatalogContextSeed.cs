using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data;

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogContext catalogContext,
        ILogger logger,
        int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (catalogContext.Database.IsSqlServer())
            {
                catalogContext.Database.Migrate();
            }

            if (!await catalogContext.CatalogBrands.AnyAsync())
            {
                await catalogContext.CatalogBrands.AddRangeAsync(
                    GetPreconfiguredCatalogBrands());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogTypes.AnyAsync())
            {
                await catalogContext.CatalogTypes.AddRangeAsync(
                    GetPreconfiguredCatalogTypes());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogItems.AnyAsync())
            {
                await catalogContext.CatalogItems.AddRangeAsync(
                    GetPreconfiguredItems());

                await catalogContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;
            
            logger.LogError(ex.Message);
            await SeedAsync(catalogContext, logger, retryForAvailability);
            throw;
        }
    }

    static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>
            {
                new("CatalogBrand1"),
                new("CatalogBrand2"),
                new("CatalogBrand3"),
                new("CatalogBrand4"),
                new("Other")
            };
    }

    static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>
            {
                new("CatalogType1"),
                new("CatalogType2"),
                new("CatalogType3"),
                new("CatalogType4")
            };
    }

    static IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
        return new List<CatalogItem>
            {
                new(2,2, "CatalogItem1", "CatalogItem1", 19.5M,  "http://catalogbaseurltobereplaced/images/products/1.png"),
                new(1,2, "CatalogItem2", "CatalogItem2", 8.50M, "http://catalogbaseurltobereplaced/images/products/2.png"),
                new(2,5, "CatalogItem3", "CatalogItem3", 12,  "http://catalogbaseurltobereplaced/images/products/3.png"),
                new(2,2, "CatalogItem4", "CatalogItem4", 12, "http://catalogbaseurltobereplaced/images/products/4.png"),
                new(3,5, "CatalogItem5", "CatalogItem5", 8.5M, "http://catalogbaseurltobereplaced/images/products/5.png"),
                new(2,2, "CatalogItem6", "CatalogItem6", 12, "http://catalogbaseurltobereplaced/images/products/6.png"),
                new(2,5, "CatalogItem7", "CatalogItem7",  12, "http://catalogbaseurltobereplaced/images/products/7.png"),
                new(2,5, "CatalogItem8", "CatalogItem8", 8.5M, "http://catalogbaseurltobereplaced/images/products/8.png"),
                new(1,5, "CatalogItem9", "CatalogItem9", 12, "http://catalogbaseurltobereplaced/images/produ.,4wqcts/9.png"),
                new(3,2, "CatalogItem10", "CatalogItem10", 12, "http://catalogbaseurltobereplaced/images/products/10.png"),
                new(3,2, "CatalogItem11", "CatalogItem11", 8.5M, "http://catalogbaseurltobereplaced/images/products/11.png"),
                new(2,5, "CatalogItem12", "CatalogItem12", 12, "http://catalogbaseurltobereplaced/images/products/12.png")
            };
    }
}
