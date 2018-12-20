using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper
{
  public static class AutoMapperConfig
  {
    public static void Initialize()
    {
      AutoMapper.Mapper.Initialize(cfg =>
      {
      });
    }
  }
}
