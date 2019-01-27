﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace AcumaticaRequest.Model
{
    /// <summary>
    /// ContractUsageTransactionDetail
    /// </summary>
    [DataContract]
    public partial class ContractUsageTransactionDetail : Entity, IEquatable<ContractUsageTransactionDetail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractUsageTransactionDetail" /> class.
        /// </summary>
        /// <param name="billingDate">billingDate.</param>
        /// <param name="branch">branch.</param>
        /// <param name="caseID">caseID.</param>
        /// <param name="date">date.</param>
        /// <param name="description">description.</param>
        /// <param name="endDate">endDate.</param>
        /// <param name="inventoryID">inventoryID.</param>
        /// <param name="qty">qty.</param>
        /// <param name="referenceNbr">referenceNbr.</param>
        /// <param name="startDate">startDate.</param>
        /// <param name="transactionID">transactionID.</param>
        /// <param name="type">type.</param>
        /// <param name="uOM">uOM.</param>
        public ContractUsageTransactionDetail(DateTimeValue billingDate = default(DateTimeValue), StringValue branch = default(StringValue), StringValue caseID = default(StringValue), DateTimeValue date = default(DateTimeValue), StringValue description = default(StringValue), DateTimeValue endDate = default(DateTimeValue), StringValue inventoryID = default(StringValue), DecimalValue qty = default(DecimalValue), StringValue referenceNbr = default(StringValue), DateTimeValue startDate = default(DateTimeValue), LongValue transactionID = default(LongValue), StringValue type = default(StringValue), StringValue uOM = default(StringValue), Guid? id = default(Guid?), long? rowNumber = default(long?), string note = default(string), Dictionary<string, Dictionary<string, CustomField>> custom = default(Dictionary<string, Dictionary<string, CustomField>>), List<FileLink> files = default(List<FileLink>)) : base(id, rowNumber, note, custom, files)
        {
            this.BillingDate = billingDate;
            this.Branch = branch;
            this.CaseID = caseID;
            this.Date = date;
            this.Description = description;
            this.EndDate = endDate;
            this.InventoryID = inventoryID;
            this.Qty = qty;
            this.ReferenceNbr = referenceNbr;
            this.StartDate = startDate;
            this.TransactionID = transactionID;
            this.Type = type;
            this.UOM = uOM;
        }

        /// <summary>
        /// Gets or Sets BillingDate
        /// </summary>
        [DataMember(Name = "BillingDate", EmitDefaultValue = false)]
        public DateTimeValue BillingDate { get; set; }

        /// <summary>
        /// Gets or Sets Branch
        /// </summary>
        [DataMember(Name = "Branch", EmitDefaultValue = false)]
        public StringValue Branch { get; set; }

        /// <summary>
        /// Gets or Sets CaseID
        /// </summary>
        [DataMember(Name = "CaseID", EmitDefaultValue = false)]
        public StringValue CaseID { get; set; }

        /// <summary>
        /// Gets or Sets Date
        /// </summary>
        [DataMember(Name = "Date", EmitDefaultValue = false)]
        public DateTimeValue Date { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name = "Description", EmitDefaultValue = false)]
        public StringValue Description { get; set; }

        /// <summary>
        /// Gets or Sets EndDate
        /// </summary>
        [DataMember(Name = "EndDate", EmitDefaultValue = false)]
        public DateTimeValue EndDate { get; set; }

        /// <summary>
        /// Gets or Sets InventoryID
        /// </summary>
        [DataMember(Name = "InventoryID", EmitDefaultValue = false)]
        public StringValue InventoryID { get; set; }

        /// <summary>
        /// Gets or Sets Qty
        /// </summary>
        [DataMember(Name = "Qty", EmitDefaultValue = false)]
        public DecimalValue Qty { get; set; }

        /// <summary>
        /// Gets or Sets ReferenceNbr
        /// </summary>
        [DataMember(Name = "ReferenceNbr", EmitDefaultValue = false)]
        public StringValue ReferenceNbr { get; set; }

        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        [DataMember(Name = "StartDate", EmitDefaultValue = false)]
        public DateTimeValue StartDate { get; set; }

        /// <summary>
        /// Gets or Sets TransactionID
        /// </summary>
        [DataMember(Name = "TransactionID", EmitDefaultValue = false)]
        public LongValue TransactionID { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "Type", EmitDefaultValue = false)]
        public StringValue Type { get; set; }

        /// <summary>
        /// Gets or Sets UOM
        /// </summary>
        [DataMember(Name = "UOM", EmitDefaultValue = false)]
        public StringValue UOM { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ContractUsageTransactionDetail {\n");
            sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append("\n");
            sb.Append("  BillingDate: ").Append(BillingDate).Append("\n");
            sb.Append("  Branch: ").Append(Branch).Append("\n");
            sb.Append("  CaseID: ").Append(CaseID).Append("\n");
            sb.Append("  Date: ").Append(Date).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  InventoryID: ").Append(InventoryID).Append("\n");
            sb.Append("  Qty: ").Append(Qty).Append("\n");
            sb.Append("  ReferenceNbr: ").Append(ReferenceNbr).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  TransactionID: ").Append(TransactionID).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  UOM: ").Append(UOM).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public override string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as ContractUsageTransactionDetail);
        }

        /// <summary>
        /// Returns true if ContractUsageTransactionDetail instances are equal
        /// </summary>
        /// <param name="input">Instance of ContractUsageTransactionDetail to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ContractUsageTransactionDetail input)
        {
            if (input == null)
                return false;

            return base.Equals(input) &&
                (
                    this.BillingDate == input.BillingDate ||
                    (this.BillingDate != null &&
                    this.BillingDate.Equals(input.BillingDate))
                ) && base.Equals(input) &&
                (
                    this.Branch == input.Branch ||
                    (this.Branch != null &&
                    this.Branch.Equals(input.Branch))
                ) && base.Equals(input) &&
                (
                    this.CaseID == input.CaseID ||
                    (this.CaseID != null &&
                    this.CaseID.Equals(input.CaseID))
                ) && base.Equals(input) &&
                (
                    this.Date == input.Date ||
                    (this.Date != null &&
                    this.Date.Equals(input.Date))
                ) && base.Equals(input) &&
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && base.Equals(input) &&
                (
                    this.EndDate == input.EndDate ||
                    (this.EndDate != null &&
                    this.EndDate.Equals(input.EndDate))
                ) && base.Equals(input) &&
                (
                    this.InventoryID == input.InventoryID ||
                    (this.InventoryID != null &&
                    this.InventoryID.Equals(input.InventoryID))
                ) && base.Equals(input) &&
                (
                    this.Qty == input.Qty ||
                    (this.Qty != null &&
                    this.Qty.Equals(input.Qty))
                ) && base.Equals(input) &&
                (
                    this.ReferenceNbr == input.ReferenceNbr ||
                    (this.ReferenceNbr != null &&
                    this.ReferenceNbr.Equals(input.ReferenceNbr))
                ) && base.Equals(input) &&
                (
                    this.StartDate == input.StartDate ||
                    (this.StartDate != null &&
                    this.StartDate.Equals(input.StartDate))
                ) && base.Equals(input) &&
                (
                    this.TransactionID == input.TransactionID ||
                    (this.TransactionID != null &&
                    this.TransactionID.Equals(input.TransactionID))
                ) && base.Equals(input) &&
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) && base.Equals(input) &&
                (
                    this.UOM == input.UOM ||
                    (this.UOM != null &&
                    this.UOM.Equals(input.UOM))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = base.GetHashCode();
                if (this.BillingDate != null)
                    hashCode = hashCode * 59 + this.BillingDate.GetHashCode();
                if (this.Branch != null)
                    hashCode = hashCode * 59 + this.Branch.GetHashCode();
                if (this.CaseID != null)
                    hashCode = hashCode * 59 + this.CaseID.GetHashCode();
                if (this.Date != null)
                    hashCode = hashCode * 59 + this.Date.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.EndDate != null)
                    hashCode = hashCode * 59 + this.EndDate.GetHashCode();
                if (this.InventoryID != null)
                    hashCode = hashCode * 59 + this.InventoryID.GetHashCode();
                if (this.Qty != null)
                    hashCode = hashCode * 59 + this.Qty.GetHashCode();
                if (this.ReferenceNbr != null)
                    hashCode = hashCode * 59 + this.ReferenceNbr.GetHashCode();
                if (this.StartDate != null)
                    hashCode = hashCode * 59 + this.StartDate.GetHashCode();
                if (this.TransactionID != null)
                    hashCode = hashCode * 59 + this.TransactionID.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                if (this.UOM != null)
                    hashCode = hashCode * 59 + this.UOM.GetHashCode();
                return hashCode;
            }
        }
    }
}
