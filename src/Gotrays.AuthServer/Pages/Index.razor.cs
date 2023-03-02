using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using BlazorComponent;
using Gotrays.Modules;

namespace Gotrays.Pages;

public partial class Index : IDisposable
{
    
        private object? _orderChart;
        private object? _profitChart;
        private object? _earningsChart;
        private object? _revenueReportChart;
        private object? _budgetChart;
        
        protected override void OnInitialized()
        {
            MasaBlazor.Breakpoint.OnUpdate += OnPropertyChanged;
            MasaBlazor.Application.PropertyChanged += OnPropertyChanged;
            _orderChart = new
            {
                Tooltip = new
                {
                    Trigger = "axis"
                },
                XAxis = new
                {
                    axisLine = new
                    {
                        Show = false
                    },
                    splitLine = new
                    {
                        Show = true,
                        LineStyle = new
                        {
                            Color = new[] { "#F0F3FA" }
                        }
                    },
                    axisLabel = new
                    {
                        Show = false
                    },
                    axisTick = new
                    {
                        Show = false
                    },
                },
                YAxis = new
                {
                    axisLine = new
                    {
                        Show = false
                    },
                    axisLabel = new
                    {
                        Show = false
                    },
                    axisTick = new
                    {
                        Show = false
                    },
                    splitLine = new
                    {
                        Show = false
                    },
                },
                Series = new[]
                {
                    new
                    {
                        name= "series-1",
                        Type= "line",
                        Data=  new int[][] { new[] { 0, 0 }, new[] { 1, 2 }, new[] { 2, 1 }, new[] { 3, 3 }, new[] { 4, 2 }, new[] { 5, 4 } },
                        Color= "#4318FF",
                        SymbolSize= 6,
                        Symbol= "circle",
                    }
                },
                Grid = new
                {
                    x = 3,
                    x2 = 3,
                    y = 3,
                    y2 = 3
                }
            };

            _profitChart = new
            {
                Tooltip = new
                {
                    Trigger = "axis",
                    axisPointer = new
                    {
                        Type = "shadow"
                    }
                },
                XAxis = new
                {
                    Data = new[] { "", "", "", "", "" },
                    axisLine = new
                    {
                        Show = false
                    },
                    axisLabel = new
                    {
                        Show = false
                    },
                    axisTick = new
                    {
                        Show = false
                    },
                    splitLine = new
                    {
                        Show = false
                    },
                },
                YAxis = new
                {
                    axisLine = new
                    {
                        Show = false
                    },
                    axisLabel = new
                    {
                        Show = false
                    },
                    axisTick = new
                    {
                        Show = false
                    },
                    splitLine = new
                    {
                        Show = false
                    },
                },
                Series = new[]
                {
                    new
                    {
                        Type= "bar",
                        Data= new[] { 2, 6, 4, 2, 4 },
                        Color= "#4318FF"
                    }
                },
                Grid = new
                {
                    x = 3,
                    x2 = 3,
                    y = 3,
                    y2 = 3
                }
            };

            int[] _earningsChartData = { 53, 31, 16 };

            _earningsChart = new
            {
                Tooltip = new
                {
                    Trigger = "item",
                },
                Series = new[]
                {
                   new
                   {
                       Type= "pie",
                       Radius= "90%",
                       Label=new
                       {
                           Show=false
                       },
                       Data=new[]
                       {
                           new
                           {
                               value= _earningsChartData[0],
                               Name= "Product",
                               ItemStyle=new
                               {
                                    Color= "#4318FF"
                               }
                           },
                           new
                           {
                               value= _earningsChartData[1],
                               Name= "App",
                               ItemStyle=new
                               {
                                    Color= "#05CD99"
                               }
                           },
                           new
                           {
                               value= _earningsChartData[2],
                               Name= "Service",
                               ItemStyle = new
                               {
                                    Color= "#FFB547"
                               }
                           },
                       }
                   }
                }
            };

            var revenueReportChartData =  new[] { new[] { 100, 180, 300, 250, 100, 50, 200, 140, 80 }, new[] { -180, -100, -70, -250, -130, -100, -90, -120 } };

            _revenueReportChart = new
            {
                Title = new
                {
                    Text = "Revenue Report",
                    TextStyle = new
                    {
                        Color = "#1B2559"
                    }
                },
                Tooltip = new
                {
                    Trigger = "axis",
                    axisPointer = new
                    {
                        Type = "shadow"
                    }
                },
                Legend = new
                {
                    Data = new[] { "Earning", "Expense" },
                    Right = "5px",
                    TextStyle = new
                    {
                        Color = "#485585",
                    }
                },
                XAxis = new
                {
                    Data = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Aug", "Sep" },
                    axisLine = new
                    {
                        Show = false,
                        LineStyle = new
                        {
                            Color = "#485585"
                        }
                    },
                    axisTick = new
                    {
                        Show = false
                    },
                    splitLine = new
                    {
                        Show = false
                    },
                    TextStyle = new
                    {
                        Color = "#485585"
                    }
                },
                YAxis = new
                {
                    axisLine = new
                    {
                        Show = false,
                        LineStyle = new
                        {
                            Color = "#485585"
                        }
                    },
                    axisTick = new
                    {
                        Show = false
                    },
                    splitLine = new
                    {
                        Show = false
                    }
                },
                Series = new[]
                {
                    new
                    {
                        Name="Earning",
                        Type= "bar",
                        Data= revenueReportChartData[0],
                        Color="#4318FF",
                        Stack="x",
                        BarWidth="15"
                    },
                    new
                    {
                        Name="Expense",
                        Type= "bar",
                        Data= revenueReportChartData[1],
                        Color="#A18BFF",
                        Stack="x",
                        BarWidth="15"
                    }
                },
                Grid = new
                {
                    y2 = 25
                }
            };

            _budgetChart = new
            {
                Radar = new[]
                {
                    new
                    {
                        Indicator=new []
                        {
                            new
                            {
                                Text="Jan",Max=300
                            },
                            new
                            {
                                Text="Feb",Max=300
                            },
                            new
                            {
                                Text="Mar",Max=300
                            },
                            new
                            {
                                Text="Apr",Max=300
                            },
                            new
                            {
                                Text="May",Max=300
                            },
                            new
                            {
                                Text="Jun",Max=300
                            },
                            new
                            {
                                Text="Aug",Max=300
                            },
                            new
                            {
                                Text="Sep",Max=300
                            },
                        },
                        Radius=70
                    }
                },
                Series = new[]
                {
                    new
                    {
                        Type= "radar",
                        Data= new []
                        {
                            new
                            {
                            }
                        },
                        Color="#4318FF",
                        SymbolSize=6,
                        Symbol="circle",
                    }
                }
            };
        }

        private Task OnPropertyChanged()
        {
            if (NavigationManager.BaseUri.EndsWith("dashboard/ecommerce"))
            {
                InvokeAsync(StateHasChanged);
            }
            return Task.CompletedTask;
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged();
        }

        public void Dispose()
        {
            MasaBlazor.Breakpoint.OnUpdate -= OnPropertyChanged;
            MasaBlazor.Application.PropertyChanged -= OnPropertyChanged;
        }
}