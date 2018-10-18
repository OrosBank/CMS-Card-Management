using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CardAppMySql.Migrations
{
    public partial class OtherTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountExternalNumber = table.Column<string>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    AccountSchemeCode = table.Column<string>(nullable: true),
                    AccountStatus = table.Column<string>(nullable: true),
                    AccountTypeId = table.Column<int>(nullable: false),
                    AccountTypeInTWCMS = table.Column<int>(nullable: false),
                    AvailableBalance = table.Column<string>(nullable: true),
                    LedgerBalance = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchName = table.Column<string>(nullable: true),
                    Sol = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardModes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccType2 = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    AccountType = table.Column<string>(nullable: true),
                    AccountTypeName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    BirthDay = table.Column<DateTime>(nullable: false),
                    BirthFIO = table.Column<string>(nullable: true),
                    BirthPlace = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    CorAddress = table.Column<string>(nullable: true),
                    CostomerRegistrationDate = table.Column<DateTime>(nullable: true),
                    Currrency = table.Column<string>(nullable: true),
                    CurrrencyCode = table.Column<string>(nullable: true),
                    CustIdFinacle = table.Column<string>(nullable: true),
                    Departmentcode = table.Column<string>(nullable: true),
                    Education = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmployeNumber = table.Column<string>(nullable: true),
                    EmploymentDate = table.Column<DateTime>(nullable: true),
                    FAX = table.Column<string>(nullable: true),
                    FIO = table.Column<string>(nullable: true),
                    Family = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    INN = table.Column<string>(nullable: true),
                    JOB = table.Column<string>(nullable: true),
                    JobPhoneNumber = table.Column<string>(nullable: true),
                    LATFIO = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    PASPlace = table.Column<string>(nullable: true),
                    PassportCode = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    ResAddresss = table.Column<string>(nullable: true),
                    Resident = table.Column<string>(nullable: true),
                    ResidentCountrycode = table.Column<string>(nullable: true),
                    Salary = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    Tittle = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    region = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "errorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ErrorDate = table.Column<DateTime>(nullable: false),
                    InnerException = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_errorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardIssuanceId = table.Column<int>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    MaskedPan = table.Column<string>(nullable: true),
                    Pan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PanUploadDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    acctno1 = table.Column<string>(nullable: true),
                    acctno2 = table.Column<string>(nullable: true),
                    card_type = table.Column<string>(nullable: true),
                    guid = table.Column<string>(nullable: true),
                    itc_id = table.Column<string>(nullable: true),
                    nameoncard = table.Column<string>(nullable: true),
                    pan = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanUploadDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PanUploads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    filename = table.Column<string>(nullable: true),
                    upload_date = table.Column<DateTime>(nullable: false),
                    uploaded_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanUploads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PinStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PinStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranCharges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchID = table.Column<int>(nullable: false),
                    CMF_Amt = table.Column<double>(nullable: false),
                    CardProductId = table.Column<int>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true),
                    DebitAmount = table.Column<double>(nullable: false),
                    DestinationAccountCMF = table.Column<string>(nullable: true),
                    LogDate = table.Column<DateTime>(nullable: true),
                    NumberOfRetries = table.Column<int>(nullable: false),
                    PstdDate = table.Column<DateTime>(nullable: true),
                    PstdFlg = table.Column<bool>(nullable: false),
                    RspCode = table.Column<string>(nullable: true),
                    SourceAccount = table.Column<string>(nullable: true),
                    traceNumber = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranCharges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardIssuances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountDocumentation = table.Column<string>(nullable: true),
                    AuthorizeBy = table.Column<int>(nullable: true),
                    AuthorizeDate = table.Column<DateTime>(nullable: true),
                    BranchBatch = table.Column<string>(nullable: true),
                    BranchId = table.Column<int>(nullable: false),
                    CardHolderResInfo = table.Column<string>(nullable: true),
                    CardPrefix = table.Column<string>(nullable: true),
                    CardStat = table.Column<string>(nullable: true),
                    CardStatusId = table.Column<int>(nullable: false),
                    CardVersion = table.Column<int>(nullable: false),
                    Card_PRG = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CurrencyNo = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    Customerstart = table.Column<string>(nullable: true),
                    DateActivated = table.Column<DateTime>(nullable: true),
                    DateRecievedAtBranchBy = table.Column<DateTime>(nullable: true),
                    DateRecievedFromProcessor = table.Column<DateTime>(nullable: true),
                    DateReleased = table.Column<DateTime>(nullable: true),
                    DateSentToBranch = table.Column<DateTime>(nullable: true),
                    DeclineComment = table.Column<string>(nullable: true),
                    DefAccountType = table.Column<string>(nullable: true),
                    Discre_DATA = table.Column<string>(nullable: true),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    HldResCode = table.Column<string>(nullable: true),
                    INSecurePINOffset = table.Column<string>(nullable: true),
                    InitiatedBy = table.Column<int>(nullable: false),
                    InitiatorAction = table.Column<string>(nullable: true),
                    IsCardActive = table.Column<bool>(nullable: false),
                    IsCardRequest = table.Column<bool>(nullable: false),
                    IsPinRequest = table.Column<bool>(nullable: false),
                    IssueRef = table.Column<string>(nullable: true),
                    MainBatch = table.Column<string>(nullable: true),
                    NameOnCard = table.Column<string>(nullable: true),
                    PAN = table.Column<string>(nullable: true),
                    PINStatusId = table.Column<int>(nullable: false),
                    PINVersion = table.Column<int>(nullable: false),
                    PickUpBranchId = table.Column<int>(nullable: false),
                    ProcessedBy = table.Column<int>(nullable: true),
                    ProcessedDate = table.Column<DateTime>(nullable: true),
                    CardProductID = table.Column<int>(nullable: false),
                    RecievedAtBranchBy = table.Column<int>(nullable: true),
                    RecievedFromProcessorBy = table.Column<int>(nullable: true),
                    ReleasedBy = table.Column<int>(nullable: true),
                    SecurePINLength = table.Column<string>(nullable: true),
                    SecurePINOffset = table.Column<string>(nullable: true),
                    SentToBranchBy = table.Column<int>(nullable: true),
                    Seq_NO = table.Column<string>(nullable: true),
                    SequenceNumber = table.Column<string>(nullable: true),
                    Track2Offset = table.Column<string>(nullable: true),
                    Track2Val = table.Column<string>(nullable: true),
                    UniqueGUID = table.Column<string>(nullable: true),
                    ValData = table.Column<string>(nullable: true),
                    ValDataQue = table.Column<string>(nullable: true),
                    WaiveCharge = table.Column<bool>(nullable: false),
                    cardAction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardIssuances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardIssuances_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardIssuances_CardStatuses_CardStatusId",
                        column: x => x.CardStatusId,
                        principalTable: "CardStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardIssuances_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardIssuances_PinStatus_PINStatusId",
                        column: x => x.PINStatusId,
                        principalTable: "PinStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardIssuanceId = table.Column<int>(nullable: true),
                    CardTypeId = table.Column<int>(nullable: false),
                    Charge = table.Column<double>(nullable: false),
                    ChargesAccount = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    FileTypeId = table.Column<int>(nullable: false),
                    IsSecAccRequired = table.Column<bool>(nullable: false),
                    ProductBin = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    VatAccount = table.Column<string>(maxLength: 15, nullable: true),
                    VatAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardProducts_CardIssuances_CardIssuanceId",
                        column: x => x.CardIssuanceId,
                        principalTable: "CardIssuances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardProducts_CardTypes_CardTypeId",
                        column: x => x.CardTypeId,
                        principalTable: "CardTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardProducts_FileType_FileTypeId",
                        column: x => x.FileTypeId,
                        principalTable: "FileType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchPINReceivedBy = table.Column<int>(nullable: false),
                    BranchPINReceivedDate = table.Column<DateTime>(nullable: false),
                    CardIssuanceId = table.Column<int>(nullable: false),
                    PINAuthorizedBy = table.Column<int>(nullable: false),
                    PINAuthorizedDate = table.Column<DateTime>(nullable: false),
                    PINDispatchedBy = table.Column<int>(nullable: false),
                    PINDispatchedDate = table.Column<DateTime>(nullable: false),
                    PINEntryDate = table.Column<DateTime>(nullable: false),
                    PINInitationBranch = table.Column<int>(nullable: false),
                    PINInitiatedBy = table.Column<int>(nullable: false),
                    PINIssuedBy = table.Column<int>(nullable: false),
                    PINIssuedDate = table.Column<DateTime>(nullable: false),
                    PINLetterAttorney = table.Column<string>(nullable: true),
                    PINPickedUpBy = table.Column<int>(nullable: false),
                    PINPickupBranch = table.Column<int>(nullable: false),
                    PINReceivedBy = table.Column<int>(nullable: false),
                    PINReceivedDate = table.Column<DateTime>(nullable: false),
                    PINStatusId = table.Column<int>(nullable: false),
                    SignStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pin_CardIssuances_CardIssuanceId",
                        column: x => x.CardIssuanceId,
                        principalTable: "CardIssuances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pin_Users_PINInitiatedBy",
                        column: x => x.PINInitiatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardIssuances_BranchId",
                table: "CardIssuances",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CardIssuances_CardStatusId",
                table: "CardIssuances",
                column: "CardStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CardIssuances_CustomerId",
                table: "CardIssuances",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CardIssuances_PINStatusId",
                table: "CardIssuances",
                column: "PINStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CardProducts_CardIssuanceId",
                table: "CardProducts",
                column: "CardIssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CardProducts_CardTypeId",
                table: "CardProducts",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CardProducts_FileTypeId",
                table: "CardProducts",
                column: "FileTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pin_CardIssuanceId",
                table: "Pin",
                column: "CardIssuanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Pin_PINInitiatedBy",
                table: "Pin",
                column: "PINInitiatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDetails");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "CardModes");

            migrationBuilder.DropTable(
                name: "CardProducts");

            migrationBuilder.DropTable(
                name: "errorLogs");

            migrationBuilder.DropTable(
                name: "PanDetails");

            migrationBuilder.DropTable(
                name: "PanUploadDetails");

            migrationBuilder.DropTable(
                name: "PanUploads");

            migrationBuilder.DropTable(
                name: "Pin");

            migrationBuilder.DropTable(
                name: "TranCharges");

            migrationBuilder.DropTable(
                name: "CardTypes");

            migrationBuilder.DropTable(
                name: "FileType");

            migrationBuilder.DropTable(
                name: "CardIssuances");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "CardStatuses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "PinStatus");
        }
    }
}
