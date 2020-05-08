using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ResearchData.Portal.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    DataUltimoAcesso = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CARACTERISTICA",
                columns: table => new
                {
                    car_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    car_descricao = table.Column<string>(nullable: true),
                    car_comun = table.Column<bool>(nullable: false),
                    car_tipo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARACTERISTICA", x => x.car_id);
                });

            migrationBuilder.CreateTable(
                name: "EXPERIMENTO",
                columns: table => new
                {
                    exp_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    exp_nome = table.Column<string>(nullable: true),
                    exp_descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPERIMENTO", x => x.exp_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJETO",
                columns: table => new
                {
                    pro_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    pro_titulo = table.Column<string>(nullable: false),
                    pro_descricao = table.Column<string>(nullable: false),
                    pro_datainicio = table.Column<DateTime>(nullable: false),
                    pro_datafim = table.Column<DateTime>(nullable: true),
                    pro_ativo = table.Column<bool>(nullable: false),
                    USUARIO_usu_id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJETO", x => x.pro_id);
                    table.ForeignKey(
                        name: "FK_PROJETO_AspNetUsers_USUARIO_usu_id",
                        column: x => x.USUARIO_usu_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GRUPODEDADOS",
                columns: table => new
                {
                    grudad_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EXPERIMENTO_exp_id = table.Column<int>(nullable: false),
                    CARACTERISTICA_car_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRUPODEDADOS", x => x.grudad_id);
                    table.ForeignKey(
                        name: "FK_GRUPODEDADOS_CARACTERISTICA_CARACTERISTICA_car_id",
                        column: x => x.CARACTERISTICA_car_id,
                        principalTable: "CARACTERISTICA",
                        principalColumn: "car_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GRUPODEDADOS_EXPERIMENTO_EXPERIMENTO_exp_id",
                        column: x => x.EXPERIMENTO_exp_id,
                        principalTable: "EXPERIMENTO",
                        principalColumn: "exp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ANALISE",
                columns: table => new
                {
                    ana_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ana_nome = table.Column<string>(nullable: true),
                    ana_descricao = table.Column<string>(nullable: true),
                    ana_datainicio = table.Column<DateTime>(nullable: false),
                    ana_datafim = table.Column<DateTime>(nullable: true),
                    ana_ativa = table.Column<bool>(nullable: false),
                    PROJETO_pro_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANALISE", x => x.ana_id);
                    table.ForeignKey(
                        name: "FK_ANALISE_PROJETO_PROJETO_pro_id",
                        column: x => x.PROJETO_pro_id,
                        principalTable: "PROJETO",
                        principalColumn: "pro_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GRUPO",
                columns: table => new
                {
                    gru_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    gru_nome = table.Column<string>(nullable: true),
                    gru_descricao = table.Column<string>(nullable: true),
                    gru_datainclusao = table.Column<DateTime>(nullable: false),
                    gru_analiseorigem = table.Column<int>(nullable: false),
                    gru_ativo = table.Column<bool>(nullable: false),
                    PROJETO_pro_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRUPO", x => x.gru_id);
                    table.ForeignKey(
                        name: "FK_GRUPO_PROJETO_PROJETO_pro_id",
                        column: x => x.PROJETO_pro_id,
                        principalTable: "PROJETO",
                        principalColumn: "pro_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COLABORADOR",
                columns: table => new
                {
                    USUARIO_usu_id = table.Column<string>(nullable: false),
                    ANALISE_ana_id = table.Column<int>(nullable: false),
                    col_datainclusao = table.Column<DateTime>(nullable: false),
                    col_acesso = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COLABORADOR", x => new { x.USUARIO_usu_id, x.ANALISE_ana_id });
                    table.ForeignKey(
                        name: "FK_COLABORADOR_ANALISE_ANALISE_ana_id",
                        column: x => x.ANALISE_ana_id,
                        principalTable: "ANALISE",
                        principalColumn: "ana_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COLABORADOR_AspNetUsers_USUARIO_usu_id",
                        column: x => x.USUARIO_usu_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "COMENTARIO",
                columns: table => new
                {
                    com_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    com_comentario = table.Column<string>(nullable: true),
                    com_datainclusao = table.Column<DateTime>(nullable: false),
                    ANALISE_ana_id = table.Column<int>(nullable: false),
                    USUARIO_usu_id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COMENTARIO", x => x.com_id);
                    table.ForeignKey(
                        name: "FK_COMENTARIO_ANALISE_ANALISE_ana_id",
                        column: x => x.ANALISE_ana_id,
                        principalTable: "ANALISE",
                        principalColumn: "ana_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COMENTARIO_AspNetUsers_USUARIO_usu_id",
                        column: x => x.USUARIO_usu_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SUJEITOEXPERIMENTAL",
                columns: table => new
                {
                    sujexp_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    sujexp_descricao = table.Column<string>(nullable: true),
                    sujexp_idexterno = table.Column<int>(nullable: true),
                    GRUPO_gru_id = table.Column<int>(nullable: true),
                    ANALISE_ana_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUJEITOEXPERIMENTAL", x => x.sujexp_id);
                    table.ForeignKey(
                        name: "FK_SUJEITOEXPERIMENTAL_ANALISE_ANALISE_ana_id",
                        column: x => x.ANALISE_ana_id,
                        principalTable: "ANALISE",
                        principalColumn: "ana_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SUJEITOEXPERIMENTAL_GRUPO_GRUPO_gru_id",
                        column: x => x.GRUPO_gru_id,
                        principalTable: "GRUPO",
                        principalColumn: "gru_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MEDICAO",
                columns: table => new
                {
                    med_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    med_datacaptacao = table.Column<DateTime>(nullable: true),
                    med_datamodificacao = table.Column<DateTime>(nullable: true),
                    med_caracteristicadouble = table.Column<double>(nullable: true),
                    med_caracteristicadata = table.Column<DateTime>(nullable: true),
                    med_caracteristicaint = table.Column<int>(nullable: true),
                    med_caracteristicastring = table.Column<string>(nullable: true),
                    med_caracteristicabool = table.Column<bool>(nullable: true),
                    SUJEITO_sujexp_id = table.Column<int>(nullable: false),
                    CARACTERISTICA_car_id = table.Column<int>(nullable: false),
                    EXPERIMENTO_exp_id = table.Column<int>(nullable: true),
                    ANALISE_ana_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICAO", x => x.med_id);
                    table.ForeignKey(
                        name: "FK_MEDICAO_ANALISE_ANALISE_ana_id",
                        column: x => x.ANALISE_ana_id,
                        principalTable: "ANALISE",
                        principalColumn: "ana_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MEDICAO_CARACTERISTICA_CARACTERISTICA_car_id",
                        column: x => x.CARACTERISTICA_car_id,
                        principalTable: "CARACTERISTICA",
                        principalColumn: "car_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MEDICAO_EXPERIMENTO_EXPERIMENTO_exp_id",
                        column: x => x.EXPERIMENTO_exp_id,
                        principalTable: "EXPERIMENTO",
                        principalColumn: "exp_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MEDICAO_SUJEITOEXPERIMENTAL_SUJEITO_sujexp_id",
                        column: x => x.SUJEITO_sujexp_id,
                        principalTable: "SUJEITOEXPERIMENTAL",
                        principalColumn: "sujexp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ANALISE_PROJETO_pro_id",
                table: "ANALISE",
                column: "PROJETO_pro_id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_COLABORADOR_ANALISE_ana_id",
                table: "COLABORADOR",
                column: "ANALISE_ana_id");

            migrationBuilder.CreateIndex(
                name: "IX_COMENTARIO_ANALISE_ana_id",
                table: "COMENTARIO",
                column: "ANALISE_ana_id");

            migrationBuilder.CreateIndex(
                name: "IX_COMENTARIO_USUARIO_usu_id",
                table: "COMENTARIO",
                column: "USUARIO_usu_id");

            migrationBuilder.CreateIndex(
                name: "IX_GRUPO_PROJETO_pro_id",
                table: "GRUPO",
                column: "PROJETO_pro_id");

            migrationBuilder.CreateIndex(
                name: "IX_GRUPODEDADOS_CARACTERISTICA_car_id",
                table: "GRUPODEDADOS",
                column: "CARACTERISTICA_car_id");

            migrationBuilder.CreateIndex(
                name: "IX_GRUPODEDADOS_EXPERIMENTO_exp_id",
                table: "GRUPODEDADOS",
                column: "EXPERIMENTO_exp_id");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICAO_ANALISE_ana_id",
                table: "MEDICAO",
                column: "ANALISE_ana_id");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICAO_CARACTERISTICA_car_id",
                table: "MEDICAO",
                column: "CARACTERISTICA_car_id");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICAO_EXPERIMENTO_exp_id",
                table: "MEDICAO",
                column: "EXPERIMENTO_exp_id");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICAO_SUJEITO_sujexp_id",
                table: "MEDICAO",
                column: "SUJEITO_sujexp_id");

            migrationBuilder.CreateIndex(
                name: "IX_PROJETO_USUARIO_usu_id",
                table: "PROJETO",
                column: "USUARIO_usu_id");

            migrationBuilder.CreateIndex(
                name: "IX_SUJEITOEXPERIMENTAL_ANALISE_ana_id",
                table: "SUJEITOEXPERIMENTAL",
                column: "ANALISE_ana_id");

            migrationBuilder.CreateIndex(
                name: "IX_SUJEITOEXPERIMENTAL_GRUPO_gru_id",
                table: "SUJEITOEXPERIMENTAL",
                column: "GRUPO_gru_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "COLABORADOR");

            migrationBuilder.DropTable(
                name: "COMENTARIO");

            migrationBuilder.DropTable(
                name: "GRUPODEDADOS");

            migrationBuilder.DropTable(
                name: "MEDICAO");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CARACTERISTICA");

            migrationBuilder.DropTable(
                name: "EXPERIMENTO");

            migrationBuilder.DropTable(
                name: "SUJEITOEXPERIMENTAL");

            migrationBuilder.DropTable(
                name: "ANALISE");

            migrationBuilder.DropTable(
                name: "GRUPO");

            migrationBuilder.DropTable(
                name: "PROJETO");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
