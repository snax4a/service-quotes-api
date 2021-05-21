using ServiceQuotes.Application.DTOs.Payment;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface IPaymentService : IDisposable
    {
        Task<List<GetPaymentResponse>> GetAllPayments(GetPaymentsFilter filter);

        Task<GetPaymentResponse> GetPaymentById(Guid id);

        Task<GetPaymentResponse> UpdatePaymentStatus(Guid id, UpdatePaymentStatusRequest dto);

        Task<List<GetPaymentResponse>> GetPaymentsByCustomerId(Guid id);

        Task<List<GetPaymentResponse>> GetPaymentsByQuoteId(Guid id);
    }
}
