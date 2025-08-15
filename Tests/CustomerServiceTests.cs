using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using ProvaPub.Models.Entities;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services;
using ProvaPub.Shared.SystemDate.Interfaces;
using Xunit;
using Xunit.Abstractions;
namespace ProvaPub.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly CustomerService _customerService;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly ITestOutputHelper _output;
        public CustomerServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _customerService = new CustomerService(
                _orderRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _dateTimeProviderMock.Object
            );

        }

        [Fact]
        public async Task CanPurchase_CustomeIdEqual_Zero()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => _customerService.CanPurchase(0, 50)
            );

            _output.WriteLine("Teste aprovado: Exception lançada para CustomerId inválido.");
        }

        [Fact]
        public async Task CanPurchase_ValueIs_Equals_Zero()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () => _customerService.CanPurchase(1, 0)
            );

            _output.WriteLine("Teste aprovado: Exception lançada para valor de compra inválido.");
        }

        [Fact]
        public async Task CanPurchase_DeveLancarErro_QuandoClienteNaoExiste()
        {
            _customerRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Customer)null);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _customerService.CanPurchase(1, 50)
            );
            _output.WriteLine("✅ Teste aprovado: Exceção lançada para cliente inexistente.");
        }


        [Fact]
        public async Task CanPurchase_DeveRetornarFalse_QuandoComprouNoUltimoMes()
        {
            _customerRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Customer { Id = 1 });

            _orderRepositoryMock
                .Setup(r => r.CountAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(1);

            var result = await _customerService.CanPurchase(1, 50);

            Assert.False(result, "❌ Falhou: Cliente deveria ser bloqueado por já ter comprado no mês.");
            _output.WriteLine("✅ Teste aprovado: Cliente foi bloqueado corretamente por compra recente.");
        }

        [Fact]
        public async Task CanPurchase_DeveRetornarFalse_QuandoPrimeiraCompraAcimaDe100()
        {
            _customerRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Customer { Id = 1 });

            _orderRepositoryMock
                .SetupSequence(r => r.CountAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(0) // Não comprou no mês
                .ReturnsAsync(0); // Nunca comprou antes

            var result = await _customerService.CanPurchase(1, 150);

            Assert.False(result, "❌ Falhou: Cliente deveria ser bloqueado por primeira compra acima de 100.");
            _output.WriteLine("✅ Teste aprovado: Cliente bloqueado corretamente por primeira compra > 100.");
        }

        [Fact]
        public async Task CanPurchase_DeveRetornarTrue_QuandoTodasRegrasPassam()
        {
            _customerRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Customer { Id = 1 });

            _orderRepositoryMock
                .SetupSequence(r => r.CountAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(0) // Não comprou no mês
                .ReturnsAsync(1); // Já comprou antes

            // Quarta-feira 10h
            _dateTimeProviderMock
                .Setup(d => d.UtcNow)
                .Returns(new DateTime(2025, 8, 13, 10, 0, 0));

            var result = await _customerService.CanPurchase(1, 50);

            Assert.True(result, "❌ Falhou: Cliente deveria estar apto a comprar.");
            _output.WriteLine("✅ Teste aprovado: Cliente pode comprar.");
        }


        [Fact]
        public async Task DeveRetornarFalse_QuandoForaDoHorarioComercial()
        {
            // Arrange: Cliente existe
            _customerRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Customer { Id = 1 });

            // Sem compras no mês
            _orderRepositoryMock
                .SetupSequence(r => r.CountAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(0) // no mês
                .ReturnsAsync(1); // já comprou antes

            // Simula sábado às 20h
            _dateTimeProviderMock
                .Setup(d => d.UtcNow)
                .Returns(new DateTime(2025, 8, 16, 20, 0, 0)); // Sábado 20:00 UTC

            // Act
            var result = await _customerService.CanPurchase(1, 50);

            // Assert
            Assert.False(result, "❌ Falhou: deveria bloquear compra fora do expediente.");
            _output.WriteLine("✅ Teste aprovado: Cliente bloqueado fora do expediente.");
        }

        [Fact]
        public async Task DeveRetornarTrue_QuandoDentroDoHorarioComercialEDiasUteis()
        {
            _customerRepositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Customer { Id = 1 });

            _orderRepositoryMock
                .SetupSequence(r => r.CountAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(0) // no mês
                .ReturnsAsync(1); // já comprou antes

            // Quarta-feira 10h
            _dateTimeProviderMock
                .Setup(d => d.UtcNow)
                .Returns(new DateTime(2025, 8, 13, 10, 0, 0));

            var result = await _customerService.CanPurchase(1, 50);

            Assert.True(result, "❌ Falhou: deveria permitir compra em horário útil.");
            _output.WriteLine("✅ Teste aprovado: Cliente pode comprar em horário útil.");
        }
    }

}
