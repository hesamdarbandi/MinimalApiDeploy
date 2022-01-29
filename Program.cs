
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<JobContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

builder.Services.AddMvcCore().AddApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.MapGet("/jobs",
   async (JobContext context) =>
   {
       return await context.Jobs.ToListAsync();
   }
).Produces<List<Job>>(StatusCodes.Status201Created).WithName("GetJobs").WithTags("Getters");

app.MapPost("/jobs",
    async ([FromBody] Job job, [FromServices] JobContext context, HttpResponse response) =>
   {
       await context.Jobs.AddAsync(job);
       await context.SaveChangesAsync();
       response.StatusCode = 201;
       //return Results.Ok()

   });

app.MapGet("/jobs/{id}",
    async (int id, JobContext context) =>
    {
        return await context.Jobs.FirstOrDefaultAsync(c => c.Id == id);
    });

    app.MapGet("/jobs/{id}",
    async (int id, JobContext context) =>
    {
        return await context.Jobs.FirstOrDefaultAsync(c => c.Id == id);
    });

app.Run();



public class Job
{
    public long Id { get; set; }
    public double Long { get; set; }
    public double Lang { get; set; }
}

class JobContext : DbContext
{
    public JobContext(DbContextOptions<JobContext> options) : base(options) { }

    public DbSet<Job> Jobs => Set<Job>();
}

