using WebData.API.Models.Dto;

namespace WebData.API.Data
{
    public static class DataCollection
    {
        public static List<DataDto>DataDetails= new List<DataDto>
            {
                new DataDto { Id = 1,Name="Asp.Net",Category="Programming Language",Price=1000},
                new DataDto { Id = 2,Name="JAVA",Category="Programming Language",Price=2000}
            };
    }
}
