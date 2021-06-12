using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
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
                    Status = "Chờ xử lý",
                    Total = 0
                };
                _context.Add(receipt);
                var count = request.ReceiptRequestDetails.Count;
                if (await _context.SaveChangesAsync() > 0)
                {
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
                        _context.Add(detail);

                    }
                    if (await _context.SaveChangesAsync() == count)
                    {
                        return count;
                    }



                }
            }
            catch { return 0; }

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

        public async Task<int> AddReceiptRequestDetailAsync(ReceiptRequestDetail receiptRequest)
        {
            _context.Add(receiptRequest);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ApproveReceiptRequestAsync(int id)
        {
            var receipt = await _context.ReceiptRequests.Where(x => x.ReceiptRequestId == id).FirstOrDefaultAsync();
            receipt.Status = RECEIPT_REQUEST_STATUS_APPROVED;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteReceiptRequest(int requesrID)
        {
            var rr = await _context.ReceiptRequests.Where(x => x.ReceiptRequestId == requesrID).FirstOrDefaultAsync();
            _context.ReceiptRequests.Remove(rr);
            return await _context.SaveChangesAsync();
        }

        public async Task<ReceiptRequest> GetReceiptRequestAsync(int id)
        {
            return await _context.ReceiptRequests.Include(x => x.ReceiptRequestDetails).Where(x => x.ReceiptRequestId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<ReceiptRequest>> GetReceiptRequestsAsync()
        {
            return await _context.ReceiptRequests.OrderBy(x => x.CreateDate).ToListAsync();
        }

        public Task<int> RejectReceiptRequestAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
