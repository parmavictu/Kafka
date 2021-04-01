using System;
using System.ComponentModel.DataAnnotations;

namespace Kafka.Infra.CrossCutting.Identity.Models.ManageViewModels
{
    public class MaritalStatusVM
    {

        public int Key { get; set; }

        public string Description { get; set; }

    }
}