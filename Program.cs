using CommanderGQL.Data;
using CommanderGQL.GraphQL;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL.Platforms;
using GraphQL.Server.Ui.Voyager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<AppDbContext>(opt => opt.UseSqlServer
            (builder.Configuration.GetConnectionString("CommandConStr")));

builder.Services
    .AddGraphQLServer()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddType<PlatformType>()
    .AddType<AddPlatformInputType>()
    .AddType<AddPlatformPayloadType>()
    .AddType<CommandType>()
    .AddType<AddCommandInputType>()
    .AddType<AddCommandPayloadType>()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions();

builder.Services.AddControllers();

var app = builder.Build();

app.UseWebSockets();

app.UseRouting();

app.MapGraphQL();

app.UseGraphQLVoyager(new GraphQLVoyagerOptions()
{
    GraphQLEndPoint = "/graphql",
    Path = "/graphql-voyager"
});

app.Run();
