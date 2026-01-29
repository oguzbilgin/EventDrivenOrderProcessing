using MediatR;

namespace EventDriven.OrderProcessing.Application.Auth.Commands.Login;
public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResponse>;
