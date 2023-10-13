using AutoMapper;
using ISession = NHibernate.ISession;
using UnitOfWork;
using bookstore.api.Models;
using MediatR;
using bookstore.api.Mediator.Queries;
using bookstore.api.ResponseMessage;
using bookstore.api.necessary.Mediator.Processors.Book;
using bookstore.api.necessary.Repositories.IRepositories.Book;
using bookstore.api.Extensions;
using bookstore.api.necessary.Repositories.IRepositories.Price;

namespace bookstore.api.necessary.Mediator.Handlers.User;

public class EditBookPriceHandler : EditBookPriceProcessor, IRequestHandler<EditBookPriceQuery, ResponseMessage<PriceModel>>
{
    private readonly IUoW _uOf;
    private readonly IPriceRepository _priceRepository;

    public EditBookPriceHandler(IUoW uOf, IPriceRepository priceRepository)
    {
        _uOf = uOf;
        _priceRepository = priceRepository;
    }

    public async Task<ResponseMessage<PriceModel>> Handle(EditBookPriceQuery request, CancellationToken cancellationToken)
    {
        return await EditBookPrice(request);
    }

    private void CommitPriceEditing(PriceModel editedPrice)
    {
        _priceRepository.Save(editedPrice);
        _uOf.Commit();
    }

    public async Task<ResponseMessage<PriceModel>> EditBookPrice(EditBookPriceQuery query)
    {
        var response = new ResponseMessage<PriceModel>();
        try
        {
            var priceToBeUpdated = _priceRepository.FindBy(query.price.Id);

            if (priceToBeUpdated.IsNull())
            {
                response.PutError(ErrorCode.PRICE_ID_DOES_NOT_EXIST, $"Could not find price with ID {query.price.Id}");

                return await Task.FromResult(response);
            }

            if(query.price.ForDay <= 0 || query.price.ForDay > 7)
            {
                response.PutError(ErrorCode.PRICE_DAY_NOT_VALID, $"Field 'ForDay' accepts only 1 - 7");

                return await Task.FromResult(response);
            }

            priceToBeUpdated.Price = query.price.Price;
            priceToBeUpdated.ForDay = query.price.ForAllDays ? 0 : query.price.ForDay;
            priceToBeUpdated.ForAllDays = query.price.ForAllDays;

            CommitPriceEditing(priceToBeUpdated);

            response.Data = priceToBeUpdated;
        }
        catch (Exception ex)
        {
            response.PutError(ErrorCode.PRICE_MODIFY_EXCEPTION, $"{ex.Message} {ex.InnerException}");

            return await Task.FromResult(response);
        }

        return await Task.FromResult(response);
    }
}