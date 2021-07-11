using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Common.Constant;

namespace Services.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly ShopDbContext _context;
        public ReceiptService(ShopDbContext shopDbContext)
        {
            _context = shopDbContext;
        }
        public async Task<bool> AddReceiptAsync( ImportWarehouse importWarehouse)
        {
            await _context.ImportWarehouses.AddAsync(importWarehouse);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<int> AddReceiptAsync(int id)
        {
            var request = await _context.ReceiptRequests.Include(x => x.ReceiptRequestDetails).Where(x => x.ReceiptRequestId == id).FirstOrDefaultAsync();
            try
            {
                var receipt = new ImportWarehouse
                {
                    CreateDate = DateTime.Now,
                    ReceiptRequestId = request.ReceiptRequestId,
                    Status = RECEIPT_STATUS_PROCESSING,
                    Total = 0,
                    UserId = request.UserId

                };
                await _context.ImportWarehouses.AddAsync(receipt);
                var countTotal = 0;
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    var count = request.ReceiptRequestDetails.Count;

                    foreach (var item in request.ReceiptRequestDetails)
                    {
                        var detail = new ImportWarehouseDetail
                        {
                            ProductDetailId = item.ProductDetailId,
                            Quantity = item.Quantity,
                            ActualQuantity = 0,
                            Status = "Chờ xử lý",
                            ImportWarehouseId = receipt.ImportWarehouseId
                        };
                        countTotal += detail.Quantity;
                        await _context.ImportWarehouseDetails.AddAsync(detail);
                    }

                    var check = await _context.SaveChangesAsync();
                    receipt.Total = countTotal;
                    _context.Update(receipt);
                    await _context.SaveChangesAsync();
                    if (check == count)
                    {
                        return count;
                    }
                }
            }
            catch 
            {
            }

            return 0;
        }

        public async Task<bool> AddReceiptDetailAsync(ImportWarehouseDetail importWarehouseDetail)
        {
            await _context.ImportWarehouseDetails.AddAsync(importWarehouseDetail);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> AddReceiptDetailRequestsAsync(IList<ReceiptRequestDetail> receiptRequestDetails)
    {
            foreach(var item in receiptRequestDetails)
            {
                await _context.ReceiptRequestDetails.AddAsync(item);
            }
            int check = await _context.SaveChangesAsync();


            if (check > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> AddReceiptRequestAsync(ReceiptRequest receiptRequest)
        {
            await _context.ReceiptRequests.AddAsync(receiptRequest);

            int check = await _context.SaveChangesAsync();

            if (check > 0)
                return true;
            else
                return false;
        }

        public async Task<int> AddReceiptRequestDetailAsync(List<ReceiptRequestDetail> receiptRequest)
        {
            await _context.ReceiptRequestDetails.AddRangeAsync(receiptRequest);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ApproveReceiptRequestAsync(int id)
        {
            var receipt = await _context.ReceiptRequests.Where(x => x.ReceiptRequestId == id).FirstOrDefaultAsync();
            receipt.Status = RECEIPT_REQUEST_STATUS_APPROVED;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CountAcceptedRequestReceiptAsync()
        {
            var result = await _context.ReceiptRequests.Where(x => x.Status == RECEIPT_REQUEST_STATUS_APPROVED).ToListAsync();
            return result.Count;
        }

        public async Task<int> DeleteReceiptRequestAsync(int requesrID)
        {
            var rr = await _context.ReceiptRequests.Where(x => x.ReceiptRequestId == requesrID).FirstOrDefaultAsync();
            _context.ReceiptRequests.Remove(rr);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> GetNumberOfProcessingReceiptAsync()
        {
            var result = await _context.ImportWarehouses.Where(x => x.Status == RECEIPT_STATUS_PROCESSING).ToListAsync();
            return result.Count;
        }

        public async Task<ReceiptRequest> GetReceiptRequestAsync(int id)
        {
            return await _context.ReceiptRequests.Include(x => x.ReceiptRequestDetails)
                .ThenInclude(x =>x.ProductDetail).ThenInclude(x => x.Product).Include(x => x.User).Where(x => x.ReceiptRequestId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<ReceiptRequest>> GetReceiptRequestsAsync()
        {
            return await _context.ReceiptRequests.Include(x => x.User).Include(x => x.ReceiptRequestDetails).OrderBy(x => x.CreateDate).ToListAsync();
        }

        public async Task<IList<PercentProcess>> GetListProcessReceiptAsync()
        {
            var result = await _context.ImportWarehouses.Include(x => x.ImportWarehouseDetails).Where(x => x.Status == RECEIPT_STATUS_PROCESSING).ToListAsync();
            var list = new List<PercentProcess>();
            foreach(var receipt in result)
            {
                var percentProcess = new PercentProcess
                {
                    RequestId = receipt.ImportWarehouseId,
                    Percent = receipt.ImportWarehouseDetails.Sum(x => x.ActualQuantity) * 100 / receipt.ImportWarehouseDetails.Sum(x => x.Quantity)
                };
                list.Add(percentProcess);
            }
            return list;
        }

        public async Task<int> RejectReceiptRequestAsync(int id)
        {
            var rr = await _context.ReceiptRequests.Where(x => x.ReceiptRequestId == id).FirstOrDefaultAsync();
            rr.Status = RECEIPT_REQUEST_REJECT;
            _context.Update(rr);
            return await _context.SaveChangesAsync();
        }

        public async Task<ImportWarehouse> GetReceiptAsync(int id)
        {
            return await _context.ImportWarehouses.Include(x => x.ImportWarehouseDetails).ThenInclude(x => x.ProductDetail)
                .ThenInclude(x => x.Product)
                .Where(x => x.ImportWarehouseId == id).FirstOrDefaultAsync();
        }

        public async Task<List<ImportWarehouse>> GetReceiptsAsync()
        {
            return await _context.ImportWarehouses.Include(x => x.ImportWarehouseDetails).OrderBy
                (x => x.Status).ToListAsync();
        }

        public async Task<ImportWarehouse> GetReceiptAfterUpdate(int id, List<int> AddQuantity)
        {
            ImportWarehouse importWarehouse = await _context.ImportWarehouses.
                Where(x => x.ImportWarehouseId == id)
                .Include(x => x.ImportWarehouseDetails)
                .ThenInclude(x => x.ProductDetail)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync();
            int i = 0, count = 0;
            var products = await _context.ProductDetails.ToListAsync();
            var listChange = new List<ProductDetail>();
             importWarehouse.Status = RECEIPT_STATUS_PROCESSING;
            
            foreach (var item in importWarehouse.ImportWarehouseDetails)
            {
                if(item.ActualQuantity + AddQuantity[i] <= item.Quantity)
                {
                    item.ActualQuantity += AddQuantity[i];
                    var productDetail = new ProductDetail();
                    productDetail = products.Where(x => x.ProductDetailId == item.ProductDetailId).FirstOrDefault();
                    productDetail.Quantity += AddQuantity[i];
                    productDetail.RemainingQuantity += AddQuantity[i];
                    listChange.Add(productDetail);
                }
                if(item.Status == RECEIPT_STATUS_WAITING)
                {
                    item.Status = RECEIPT_STATUS_PROCESSING;
                }
                
                if(item.ActualQuantity == item.Quantity)
                {
                    item.Status = RECEIPT_STATUS_COMPLETE;
                    count++;
                }
                
            }
            if (count == importWarehouse.ImportWarehouseDetails.Count)
            {
                importWarehouse.Status = RECEIPT_STATUS_COMPLETE;
            }
            _context.ProductDetails.UpdateRange(listChange);
            _context.ImportWarehouses.Update(importWarehouse);
            await _context.SaveChangesAsync();

            return importWarehouse;

        }
    }
}
