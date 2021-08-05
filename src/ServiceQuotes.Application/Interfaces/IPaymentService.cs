﻿using ServiceQuotes.Application.DTOs.Payment;
using ServiceQuotes.Application.DTOs.Paynow;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface IPaymentService : IDisposable
    {
        Task<List<GetPaymentResponse>> GetAllPayments(GetPaymentsFilter filter);

        Task<GetPaymentResponse> GetPaymentById(Guid id);

        Task<List<GetPaymentResponse>> GetPaymentsByCustomerId(Guid id);

        Task<List<GetPaymentResponse>> GetPaymentsByQuoteId(Guid id);

        Task<GetPaymentResponse> GetPaymentForQuote(Guid quoteId);

        Task<CreatePaymentResponse> CreatePaymentForQuote(CreatePaymentRequest dto, Guid accountId);

        Task<bool> ProcessPaynowNotification(PaynowNotificationRequest dto, string signature);
    }
}
