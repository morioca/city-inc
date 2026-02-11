namespace Domain.Models
{
    /// <summary>
    /// Immutable value object representing the allocation across all six policy categories.
    /// </summary>
    public class BudgetAllocation
    {
        /// <summary>
        /// Allocation for Welfare &amp; Healthcare category.
        /// </summary>
        public long WelfareHealthcare { get; }

        /// <summary>
        /// Allocation for Education &amp; Childcare category.
        /// </summary>
        public long EducationChildcare { get; }

        /// <summary>
        /// Allocation for Industry Development category.
        /// </summary>
        public long IndustryDevelopment { get; }

        /// <summary>
        /// Allocation for Infrastructure category.
        /// </summary>
        public long Infrastructure { get; }

        /// <summary>
        /// Allocation for Disaster Prevention &amp; Safety category.
        /// </summary>
        public long DisasterPrevention { get; }

        /// <summary>
        /// Allocation for Tourism &amp; Culture category.
        /// </summary>
        public long TourismCulture { get; }

        /// <summary>
        /// Gets the total amount allocated across all categories.
        /// </summary>
        public long TotalAllocated =>
            WelfareHealthcare + EducationChildcare + IndustryDevelopment +
            Infrastructure + DisasterPrevention + TourismCulture;

        /// <summary>
        /// Create a new BudgetAllocation with the specified values for all categories.
        /// </summary>
        /// <param name="welfareHealthcare">Allocation for Welfare &amp; Healthcare</param>
        /// <param name="educationChildcare">Allocation for Education &amp; Childcare</param>
        /// <param name="industryDevelopment">Allocation for Industry Development</param>
        /// <param name="infrastructure">Allocation for Infrastructure</param>
        /// <param name="disasterPrevention">Allocation for Disaster Prevention &amp; Safety</param>
        /// <param name="tourismCulture">Allocation for Tourism &amp; Culture</param>
        public BudgetAllocation(
            long welfareHealthcare,
            long educationChildcare,
            long industryDevelopment,
            long infrastructure,
            long disasterPrevention,
            long tourismCulture)
        {
            WelfareHealthcare = welfareHealthcare;
            EducationChildcare = educationChildcare;
            IndustryDevelopment = industryDevelopment;
            Infrastructure = infrastructure;
            DisasterPrevention = disasterPrevention;
            TourismCulture = tourismCulture;
        }

        /// <summary>
        /// Create a budget allocation with equal distribution across all six categories.
        /// </summary>
        /// <param name="totalBudget">The total budget to distribute</param>
        /// <returns>A new BudgetAllocation with equal distribution</returns>
        public static BudgetAllocation EqualDistribution(long totalBudget)
        {
            long perCategory = totalBudget / 6;
            return new BudgetAllocation(
                perCategory,
                perCategory,
                perCategory,
                perCategory,
                perCategory,
                perCategory);
        }
    }
}
