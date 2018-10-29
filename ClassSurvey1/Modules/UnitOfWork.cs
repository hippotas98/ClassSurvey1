
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassSurvey1.Modules
{
    public interface IUnitOfWork : ITransientService/*, IDisposable*/
    {
        //void Complete();
        //    ICarrierRepository CarrierRepository { get; }
        //    ICategoryRepository CategoryRepository { get; }
        //    ICountryRepository CountryRepository { get; }
        //    ICouponRepository CouponRepository { get; }
        //    ICustomerRepository CustomerRepository { get; }
        //    ICustomerGroupRepository CustomerGroupRepository { get; }
        //    IDiscountRepository DiscountRepository { get; }
        //    IEmployeeRepository EmployeeRepository { get; }
        //    IInputRepository InputRepository { get; }
        //    IIntroductionRepository IntroductionRepository { get; }
        //    IInventoryRepository InventoryRepository { get; }
        //    IInventoryCheckpointRepository InventoryCheckpointRepository { get; }
        //    IInvoiceRepository InvoiceRepository { get; }
        //    IIssueNoteRepository IssueNoteRepository { get; }
        //    ILanguageRepository LanguageRepository { get; }
        //    IManufacturerRepository ManufacturerRepository { get; }
        //    IOperationRepository OperationRepository  { get; }
        //    IOrderRepository OrderRepository { get; }
        //    IOutputRepository OutputRepository { get; }
        //    IPackRepository PackRepository { get; }
        //    IPermissionRepository PermissionRepository { get; }
        //    IProductRepository ProductRepository { get; }
        //    IReceiptNoteRepository ReceiptNoteRepository { get; }
        //    IRoleRepository RoleRepository { get; }
        //    IShipmentDetailRepository ShipmentDetailRepository { get; }
        //    ISupplierRepository SupplierRepository { get; }
        //    IWareHouseRepository WareHouseRepository { get; }
        }
        public class UnitOfWork : IUnitOfWork
        {
    //    private EShopContext context;
    //    private IDbContextTransaction _transaction;

        //    private ICarrierRepository _carrierRepository;
        //    private ICategoryRepository _categoryRepository;
        //    private ICountryRepository _countryRepository;
        //    private ICouponRepository _couponRepository;
        //    private ICustomerRepository _customerRepository;
        //    private ICustomerGroupRepository _customerGroupRepository;
        //    private IDiscountRepository _discountRepository;
        //    private IEmployeeRepository _employeeRepository;
        //    private IInputRepository _inputRepository;
        //    private IIntroductionRepository _introductionRepository;
        //    private IInventoryRepository _inventoryRepository;
        //    private IInventoryCheckpointRepository _inventoryCheckpointRepository;
        //    private IInvoiceRepository _invoiceRepository;
        //    private IIssueNoteRepository _issueNoteRepository;
        //    private ILanguageRepository _languageRepository;
        //    private IManufacturerRepository _manufacturerRepository;
        //    private IOperationRepository _operationRepository;
        //    private IOrderRepository _orderRepository;
        //    private IOutputRepository _outputRepository;
        //    private IPackRepository _packRepository;
        //    private IPermissionRepository _permissionRepository;
        //    private IProductRepository _productRepository;
        //    private IReceiptNoteRepository _receiptNoteRepository;
        //    private IRoleRepository _roleRepository;
        //    private IShipmentDetailRepository _shipmentDetailRepository;
        //    private ISupplierRepository _supplierRepository;
        //    private IWareHouseRepository _wareHouseRepository;

        //private void InitTransaction()
        //{
        //    if (_transaction == null)
        //        _transaction = context.Database.BeginTransaction();
        //}

        public UnitOfWork()
        {
            //this.context = new EShopContext();
        }

        //public UnitOfWork(EShopContext context)
        //{
        //    this.context = context;
        //}

        //~UnitOfWork()
        //{
        //    context.Dispose();
        //}
        //public void Dispose()
        //{
        //    context.Dispose();
        //}
        //public void Complete()
        //{
        //    try
        //    {
        //        context.SaveChanges();
        //        _transaction.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        _transaction.Rollback();
        //        throw new InternalServerErrorException(ex.Message);
        //    }
        //    finally
        //    {
        //        _transaction.Dispose();
        //        _transaction = null;
        //    }
        //}

        //public ICarrierRepository CarrierRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_carrierRepository == null) _carrierRepository = new CarrierRepository(context);
        //        return _carrierRepository;
        //    }
        //}
        //public ICategoryRepository CategoryRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_categoryRepository == null) _categoryRepository = new CategoryRepository(context);
        //        return _categoryRepository;
        //    }
        //}

        //public ICountryRepository CountryRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_countryRepository == null) _countryRepository = new CountryRepository(context);
        //        return _countryRepository;
        //    }
        //}

        //public ICouponRepository CouponRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_couponRepository == null) _couponRepository = new CouponRepository(context);
        //        return _couponRepository;
        //    }
        //}

        //public ICustomerRepository CustomerRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_customerRepository == null) _customerRepository = new CustomerRepository(context);
        //        return _customerRepository;
        //    }
        //}

        //public ICustomerGroupRepository CustomerGroupRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_customerGroupRepository == null) _customerGroupRepository = new CustomerGroupRepository(context);
        //        return _customerGroupRepository;
        //    }
        //}

        //public IDiscountRepository DiscountRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_discountRepository == null) _discountRepository = new DiscountRepository(context);
        //        return _discountRepository;
        //    }
        //}

        //public IEmployeeRepository EmployeeRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_employeeRepository == null) _employeeRepository = new EmployeeRepository(context);
        //        return _employeeRepository;
        //    }
        //}

        //public IInputRepository InputRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_inputRepository == null) _inputRepository = new InputRepository(context);
        //        return _inputRepository;
        //    }
        //}

        //public IIntroductionRepository IntroductionRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_introductionRepository == null) _introductionRepository = new IntroductionRepository(context);
        //        return _introductionRepository;
        //    }
        //}

        //public IInventoryRepository InventoryRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_inventoryRepository == null) _inventoryRepository = new InventoryRepository(context);
        //        return _inventoryRepository;
        //    }
        //}

        //public IInventoryCheckpointRepository InventoryCheckpointRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_inventoryCheckpointRepository == null) _inventoryCheckpointRepository = new InventoryCheckpointRepository(context);
        //        return _inventoryCheckpointRepository;
        //    }
        //}

        //public IInvoiceRepository InvoiceRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_invoiceRepository == null) _invoiceRepository = new InvoiceRepository(context);
        //        return _invoiceRepository;
        //    }
        //}

        //public IIssueNoteRepository IssueNoteRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_issueNoteRepository == null) _issueNoteRepository = new IssueNoteRepository(context);
        //        return _issueNoteRepository;
        //    }
        //}

        //public ILanguageRepository LanguageRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_languageRepository == null) _languageRepository = new LanguageRepository(context);
        //        return _languageRepository;
        //    }
        //}

        //public IManufacturerRepository ManufacturerRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_manufacturerRepository == null) _manufacturerRepository = new ManufacturerRepository(context);
        //        return _manufacturerRepository;
        //    }
        //}

        //public IOperationRepository OperationRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_operationRepository == null) _operationRepository = new OperationRepository(context);
        //        return _operationRepository;
        //    }
        //}

        //public IOrderRepository OrderRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_orderRepository == null) _orderRepository = new OrderRepository(context);
        //        return _orderRepository;
        //    }
        //}


        //public IOutputRepository OutputRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_outputRepository == null) _outputRepository = new OutputRepository(context);
        //        return _outputRepository;
        //    }
        //}

        //public IPackRepository PackRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_packRepository == null) _packRepository = new PackRepository(context);
        //        return _packRepository;
        //    }
        //}

        //public IPermissionRepository PermissionRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_permissionRepository == null) _permissionRepository = new PermissionRepository(context);
        //        return _permissionRepository;
        //    }
        //}

        //public IProductRepository ProductRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_productRepository == null) _productRepository = new ProductRepository(context);
        //        return _productRepository;
        //    }
        //}


        //public IReceiptNoteRepository ReceiptNoteRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_receiptNoteRepository == null) _receiptNoteRepository = new ReceiptNoteRepository(context);
        //        return _receiptNoteRepository;
        //    }
        //}


        //public IRoleRepository RoleRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_roleRepository == null) _roleRepository = new RoleRepository(context);
        //        return _roleRepository;
        //    }
        //}

        //public IShipmentDetailRepository ShipmentDetailRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_shipmentDetailRepository == null) _shipmentDetailRepository = new ShipmentDetailRepository(context);
        //        return _shipmentDetailRepository;
        //    }
        //}

        //public ISupplierRepository SupplierRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_supplierRepository == null) _supplierRepository = new SupplierRepository(context);
        //        return _supplierRepository;
        //    }
        //}

        //public IWareHouseRepository WareHouseRepository
        //{
        //    get
        //    {
        //        InitTransaction();
        //        if (_wareHouseRepository == null) _wareHouseRepository = new WareHouseRepository(context);
        //        return _wareHouseRepository;
        //    }
        //}
    }
}
