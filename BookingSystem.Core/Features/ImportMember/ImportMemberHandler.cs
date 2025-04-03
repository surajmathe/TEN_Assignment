using BookingSystem.Core.Common;
using BookingSystem.Core.CsvMappings;
using BookingSystem.DataAccessLayer.Entity;
using BookingSystem.DataAccessLayer.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;

namespace BookingSystem.Core.Features.ImportMember
{
    public class ImportMemberHandler : IRequestHandler<ImportMemberRequest, BaseResult>
    {
        private readonly IMemberRepository _memberRepository;
        public ImportMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public async Task<BaseResult> Handle(ImportMemberRequest request, CancellationToken cancellationToken)
        {
            BaseResult result = new();
            List<Member> members = new();
            try
            {
                CsvParser<Member> csvParser = new(new CsvParserOptions(true, ','), new CsvMemberMapping());
                var parsingResult = csvParser
                    .ReadFromStream(request.File, Encoding.ASCII)
                    .ToList();
                if (parsingResult != null && parsingResult.Count > 0)
                {
                    parsingResult.ForEach(item =>
                    {
                        if (item.IsValid)
                            members.Add(item.Result);
                    });
                }
            }
            catch
            {
                result.Message = "Failed to parse the CSV file.";
                return result;
            }

            if (members.Count == 0)
            {
                result.Message = "No valid records found in the CSV";
                return result;
            }

            result.Succeeded = await this._memberRepository.AddBulkMembers(members, cancellationToken);

            return result;
        }
    }
}
