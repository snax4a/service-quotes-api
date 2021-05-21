using AutoMapper;
using LinqKit;
using ServiceQuotes.Application.DTOs.Payment;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetPaymentResponse>> GetAllPayments(GetPaymentsFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<Payment>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.Provider.Equals(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.TransactionId.Equals(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.Status.Equals(filter.SearchString.ToLower()));
            }

            var payment = await _unitOfWork.Payments.Find(predicate);

            return _mapper.Map<List<GetPaymentResponse>>(payment);
        }

        public async Task<GetPaymentResponse> GetPaymentById(Guid id)
        {
            return _mapper.Map<GetPaymentResponse>(await _unitOfWork.Payments.Get(id));
        }

        public async Task<GetPaymentResponse> CreatePayment(CreatePaymentRequest dto)
        {
            // validate
            if (await _unitOfWork.Payments.Get(dto.TransactionId) is not null)
                throw new KeyNotFoundException($"Payment with '{dto.TransactionId}' already exists.");

            // map dto to new payment object
            var newPayment = _mapper.Map<Payment>(dto);

            _unitOfWork.Payments.Add(newPayment);
            _unitOfWork.Commit();

            return _mapper.Map<GetPaymentResponse>(newPayment);
        }

        public async Task<GetPaymentResponse> UpdatePaymentStatus(Guid id, UpdatePaymentStatusRequest dto)
        {
            var payment = await _unitOfWork.Payments.Get(id);

            // validate
            if (payment is null) throw new KeyNotFoundException();

            if (payment.Status != dto.Status)
                payment.Status = dto.Status;

            _unitOfWork.Commit();
            return _mapper.Map<GetPaymentResponse>(payment);
        }

        public async Task<List<GetPaymentResponse>> GetPaymentsByCustomerId(Guid id)
        {
            return _mapper.Map<List<GetPaymentResponse>>(await _unitOfWork.Payments.Find(p => p.CustomerId.Equals(id)));
        }

        public async Task<List<GetPaymentResponse>> GetPaymentsByQuoteId(Guid id)
        {
            return _mapper.Map<List<GetPaymentResponse>>(await _unitOfWork.Payments.Find(p => p.QuoteId.Equals(id)));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
