using MediatR;

namespace Core.CQRS;

public interface IQuery : IRequest<Unit>;
public interface IQuery<out T>: IRequest<T>
where T: notnull;