namespace GPConnect.Provider.AcceptanceTests.Steps
{
	using Constants;
	using Context;
	using TechTalk.SpecFlow;
	using Shouldly;
	using Hl7.Fhir.Model;
	using System.Collections.Generic;
	using System;
	using System.Linq;
	using GPConnect.Provider.AcceptanceTests.Enum;
	using static Hl7.Fhir.Model.Parameters;
	using GPConnect.Provider.AcceptanceTests.Helpers;
	using GPConnect.Provider.AcceptanceTests.Steps;
	using GPConnect.Provider.AcceptanceTests.Logger;
	using System.Text.RegularExpressions;

	[Binding]
	public sealed class StructuredAllergySteps : BaseSteps
	{
		private readonly HttpContext _httpContext;
		private List<AllergyIntolerance> AllAllergyIntolerances = new List<AllergyIntolerance>();
		private List<AllergyIntolerance> ActiveAllergyIntolerances => _httpContext.FhirResponse.AllergyIntolerances;
		private List<List> Lists => _httpContext.FhirResponse.Lists;
		private Bundle Bundle => _httpContext.FhirResponse.Bundle;

		public StructuredAllergySteps(HttpSteps httpSteps, HttpContext httpContext)
			: base(httpSteps)
		{
			_httpContext = httpContext;
		}

		#region Allergy Parameters

		[Given(@"I add the allergies parameter with resolvedAllergies set to ""(.*)""")]
		public void GivenIAddTheAllergiesParameterWithResolvedAllergiesSetTo(string partValue)
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] { Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(Boolean.Parse(partValue))) };
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
		}

		[Given(@"I add the allergies parameter without mandatory part parameter")]
		public void GivenIAddTheAllergiesParameterWithoutMandatoryPartParameter()
		{
			ParameterComponent param = new ParameterComponent();
			param.Name = FhirConst.GetStructuredRecordParams.kAllergies;
			_httpContext.HttpRequestConfiguration.BodyParameters.Parameter.Add(param);
		}

		[Given(@"I add an unknown allergies parameter name")]
		public void GivenIAddAnUnknownAllergiesParameterName()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add("inlcudeUnknownAllergies", tuples);
		}

		[Given(@"I add the allergies parameter with mandatory part parameter and includePrescriptionIssues")]
		public void GivenIAddTheAllergiesParameterWithMandatoryPartParameterAndIncludePrescriptionIssues()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kPrescriptionIssues, (Base)new FhirBoolean(false)),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
		}

		[Given(@"I add the allergies parameter with mandatory parameter and additional parameter")]
		public void GivenIAddTheAllergiesParameterWithMandatoryParameterAndAdditionalParameter()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetDefaultTimePeriod()),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
		}

		[Given(@"I add the allergies parameter with mandatory part parameter start date")]
		public void GivenIAddTheAllergiesParameterWithMandatoryPartParameterStartDate()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetTimePeriodStartDateOnly()),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
		}

		[Given(@"I add the allergies parameter with an end date")]
		public void GivenIAddTheAllergiesParameterWithAnEndPeriod()
		{
			IEnumerable<Tuple<string, Base>> tuples = new Tuple<string, Base>[] {
				Tuple.Create(FhirConst.GetStructuredRecordParams.kTimePeriod, (Base)TimePeriodHelper.GetTimePeriodEndDateOnly()),
				Tuple.Create(FhirConst.GetStructuredRecordParams.kResolvedAllergies, (Base)new FhirBoolean(false))
			};
			_httpContext.HttpRequestConfiguration.BodyParameters.Add(FhirConst.GetStructuredRecordParams.kAllergies, tuples);
		}

		#endregion

		#region List and Bundle Validity Checks

		[Then(@"the List of AllergyIntolerances should be valid")]
		public void TheListOfAllergyIntolerancesShouldBeValid()
		{
			Lists.ForEach(list =>
			{
				//make sure to only act on allergies lists
				if (list.Code.Coding.First().Code.Equals(FhirConst.GetSnoMedParams.kActiveAllergies) || list.Code.Coding.First().Code.Equals(FhirConst.GetSnoMedParams.kResolvedAllergies))
				{
					AccessRecordSteps.BaseListParametersAreValid(list);

					// Added 1.2.1 RMB 1/10/2018
					list.Meta.VersionId.ShouldBeNull("List Meta VersionId should be Null");
					list.Meta.LastUpdated.ShouldBeNull("List Meta LastUpdated should be Null");

					//Alergy specific checks
					CheckForValidMetaDataInResource(list, FhirConst.StructureDefinitionSystems.kList);

					if (list.Title.Equals(FhirConst.ListTitles.kActiveAllergies))
					{
						list.Code.Coding.First().Code.Equals("886921000000105");
					}
					else if (list.Title.Equals(FhirConst.ListTitles.kResolvedAllergies))
					{
						// Changed from TBD to 1103671000000101 for 1.2.0 RMB 7/8/2018
						list.Code.Coding.First().Code.Equals("1103671000000101");
						// Amended github ref 89
						// RMB 9/10/2018				
						// git hub ref 174 snomed code display set to Ended allergies
						// RMB 23/1/19
						//					list.Code.Coding.First().Display.ShouldBe("Ended allergies (record artifact)");
						list.Code.Coding.First().Display.ShouldBe("Ended allergies");
					}


					if (list.Entry.Count > 0)
					{
						list.Entry.ForEach(entry =>
						{
							entry.Item.ShouldNotBeNull("The item field must be populated for each list entry.");
							entry.Item.Reference.ShouldMatch("AllergyIntolerance/.+|#.+");
							if (entry.Item.IsContainedReference)
							{
								string id = entry.Item.Reference.Substring(1);
								List<Resource> contained = list.Contained.Where(allergy => allergy.Id.Equals(id)).ToList();
								contained.Count.ShouldBe(1);
								contained.ForEach(allergy =>
								{
									AllergyIntolerance allergyIntolerance = (AllergyIntolerance)allergy;
									allergyIntolerance.ClinicalStatus.Equals(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Resolved);
								});
							}
						});
					}

					if (list.Entry.Count == 0)
					{
						list.EmptyReason.ShouldNotBeNull("The List's empty reason field must be populated if the list is empty.");
						list.Note.ShouldNotBeNull("The List's note field must be populated if the list is empty.");
					}
				}
			});
		}


		[Then(@"the Bundle should contain ""(.*)"" allergies")]
		public void TheBundleShouldContainAllergies(int number)
		{
			ActiveAllergyIntolerances.Count.ShouldBe(number, "An incorrect number of allergies was returned for the patient.");
		}

		[Then(@"the Bundle should contain ""(.*)"" lists")]
		public void TheBundleShouldContainLists(int number)
		{
			Lists.Count.ShouldBe(number, "An incorrect number of lists was returned with the bundle.");
		}

		[Then(@"the Bundle should contain a list with the title ""(.*)""")]
		public void TheBundleShouldContainAListWithTheTitleAndEntries(string title)
		{
			getListsWithTitle(title).ShouldHaveSingleItem();
		}

		[Then(@"the Bundle should contain the correct number of allergies")]
		public void TheBundleShouldContainTheCorrectNumberOfAllergies()
		{
			List active = Lists.Where(list => list.Title.Equals(FhirConst.ListTitles.kActiveAllergies)).ToList().First();

			ActiveAllergyIntolerances.Count.ShouldBe(active.Entry.Count);
		}

		[Then(@"the Bundle should not contain a list with the title ""(.*)""")]
		public void TheBundleShouldNotContainAListWithTheTitle(string title)
		{
			getListsWithTitle(title).ShouldBeEmpty();
		}

		private List<List> getListsWithTitle(string title)
		{
			return Lists
					.Where(list => list.Title.Equals(title))
					.ToList();
		}

		[Then(@"the Lists are valid for a patient with no allergies")]
		public void TheListsAreValidForAPatientWithNoAllergies()
		{
			Lists.ForEach(list =>
		  {
			  list.Entry.ShouldBeEmpty();
			  list.Note.ShouldNotBeNull();
			  list.Note.ShouldHaveSingleItem();
			  //#289 PG 6/9/2019 - changed as more notes added
			  //list.Note.First().Text.ShouldBe("Information not available");
			  var found = false;
			  foreach (var note in list.Note)
			  {
				  if (note.Text.Contains("Information not available"))
					  found = true;
			  }

			  if (!found)
			  {
				  Log.WriteLine("Warning not Found : Information not available");
				  found.ShouldBeTrue("Warning not Found : Information not available");
			  }
			  else
			  {
				  Log.WriteLine("Warning Found : Information not available");
			  }

			  list.EmptyReason.ShouldNotBeNull();
			  list.EmptyReason.Coding.Count.ShouldBe(1);
			  // git hub ref 158
			  // RMB 9/1/19			   
			  list.EmptyReason.Coding.First().System.ShouldBe(FhirConst.StructureDefinitionSystems.kListEmptyReason);
			  // Amended for github ref 87
			  // RMB 9/10/2018			   
			  list.EmptyReason.Coding.First().Code.ShouldBe("no-content-recorded");
			  // Amended for github ref 172
			  // RMB 24/1/19			   			   
			  list.EmptyReason.Coding.First().Display.ShouldBe("No Content Recorded");
		  });
		}

		[Then(@"the Lists are valid for a patient with explicit no allergies coding")]
		public void TheListsAreValidForAPatientWithExplicitNoAllergiesCoding()
		{
			Lists.ForEach(list =>
			{
				if (list.Title.Equals(FhirConst.ListTitles.kActiveAllergies))
				{
					ActiveAllergyIntolerances.Count.Equals(1);
					ActiveAllergyIntolerances.ForEach(allergy =>
					{
						allergy.Code.Coding.First().Code.ShouldNotBeNull();
					});
				}
			});
		}

		[Then(@"the Lists are valid for a patient with allergies")]
		public void TheListsAreValidForAPatientWithAllergies()
		{
			Lists.ForEach(list =>
			{
				if (list.Code.Coding.First().Code.Equals(FhirConst.GetSnoMedParams.kActiveAllergies))
				{

					list.EmptyReason.ShouldBeNull("List empty reason should be null");
					list.Note.ForEach(note =>
					{
						note.Text.ShouldNotContain("no known allergies");
					});
				}
			});
		}

		[Then(@"the Lists are valid for a patient without allergies")]
		public void TheListsAreValidForAPatientWithooutAllergies()
		{
			Lists.ForEach(list =>
			{
				//list.EmptyReason.ShouldNotBeNull();
				list.Extension.ForEach(extension =>
				{
					extension.Url.ShouldNotBeNull();
				});
				//list.Note.ForEach(note =>
				//{
				//    note.Text.ShouldNotContain("no known allergies");
				//});
			});
		}


		[Then(@"Check the list contains the following warning ""(.*)""")]
		public void CheckTheListContainsTheFollowingWarning(string WarningToCheckFor)
		{
			Lists.ForEach(list =>
			{
				var found = list.Extension
					.Where(extension => extension.Url.Equals(FhirConst.StructureDefinitionSystems.kExtListWarningCode))
					.Where(extension => extension.Value.ToString().Equals(WarningToCheckFor)).ToList();

				if (found.Count() == 1)
					Log.WriteLine("Found Warning : " + WarningToCheckFor + " in List : " + list.Title);

				found.Count().ShouldBe(1, "Unable to Find Warning : " + WarningToCheckFor + " in List : " + list.Title);
			});
		}


		[Then(@"Check the warning ""(.*)"" has associated note ""(.*)""")]
		public void CheckTheListContainsTheFollowingNote(string warning, string noteToCheckFor)
		{
			Lists.ForEach(list =>
			{
				if (warning != "data-in-transit")
				{
					var matches = list.Note
					.Where(note => note.Text.Contains(noteToCheckFor));

					if (matches.Count() == 1)
						Log.WriteLine("Found Note : " + noteToCheckFor + " in List : " + list.Title);

					matches.Count().ShouldBe(1, "Unable to Find Note : " + noteToCheckFor + "in LIst : " + list.Title);
				}
				//Process data-in-transit separatly due to date being variable in message
				else
				{
					Regex regex = new Regex("(.*)dd-Mmm-yyyy(.*)");
					string noteWithRegex = regex.Replace(noteToCheckFor, "$1.*$2");
					Regex findNoteRegex = new Regex(noteWithRegex);

					var found = false;
					MatchCollection matches;
					foreach (var note in list.Note)
					{
						matches = findNoteRegex.Matches(note.Text);
						if (matches.Count == 1)
						{
							found = true;
							Log.WriteLine("Warning Note Found : " + noteToCheckFor + " in List : " + list.Title);
						}
					}

					if (!found)
					{
						found.ShouldBeTrue("Unable to Find Warning Note : " + noteToCheckFor + " in LIst : " + list.Title);
						Log.WriteLine("Note Not Found For Warning: " + noteToCheckFor + " in List : " + list.Title);
					}

				}
			});
		}





		#endregion

		#region Allergy Intolerance Validity Checks

		[Then(@"the AllergyIntolerance should be valid")]
		public void TheAllergyIntoleranceShouldBeValid()
		{
			AllAllergyIntolerances.AddRange(ActiveAllergyIntolerances);

			//Get the 'contained' resolved allergies from the resolved list
			List<List> resolved = Lists.Where(list => list.Title.Equals(FhirConst.ListTitles.kResolvedAllergies)).ToList();
			if (resolved.Count > 0)
			{
				List<Resource> resolvedAllergies = resolved.First().Contained.Where(resource => resource.ResourceType.Equals(ResourceType.AllergyIntolerance)).ToList();
				resolvedAllergies.ForEach(resource =>
				{
					AllergyIntolerance allergy = (AllergyIntolerance)resource;
					AllAllergyIntolerances.Add(allergy);
				});
			}

			TheAllergyIntoleranceRecorderShouldbeValid();
			TheAllergyIntoleranceClinicalStatusShouldbeValid();
			TheAllergyIntoleranceAssertedDateShouldBeValid();
			TheAllergyIntoleranceIdShouldBeValid();

			// Added check for Allergy Identifier should be set and be a GUID RMB 07-08-2016
			TheAllergyIntoleranceIdentifierShouldBeValid();

			TheAllergyIntoleranceMetadataShouldBeValid();
			TheAllergyIntolerancePatientShouldBeValidAndResolvable();
			TheAllergyIntoleranceVerificationStatusShouldbeValid();
			TheAllergyIntoleranceReactionShouldBeValid();
			TheAllergyIntoleranceEndDateShouldBeValid();
			TheAllergyIntoleranceCodeShouldbeValid();
			TheListOfAllergyIntolerancesShouldBeValid();
			TheAllergyIntoleranceCategoryShouldbeValid();
			TheAllergyIntoleranceEncounterShouldBeValid();
			// Added 1.2.1 RMB 1/10/2018
			TheAllergyIntoleranceNotInUseShouldBeValid();
		}

		private void TheAllergyIntoleranceEncounterShouldBeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				Extension encounter = allergy.GetExtension(FhirConst.StructureDefinitionSystems.kExtEncounter);
				if (encounter != null)
				{
					ResourceReference encRef = (ResourceReference)encounter.Value;
					encRef.Reference.StartsWith("Encounter/");
				}
			});
		}

		private void TheAllergyIntoleranceRecorderShouldbeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				allergy.Recorder.ShouldNotBeNull("AllergyIntolerance Recorder cannot be null");
			});
		}

		private void TheAllergyIntoleranceMetadataShouldBeValid()
		{
			AllAllergyIntolerances.ForEach(allergyIntolerance =>
			{
				CheckForValidMetaDataInResource(allergyIntolerance, FhirConst.StructureDefinitionSystems.kAllergyIntolerance);
			});
		}

		private void TheAllergyIntoleranceIdShouldBeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				allergy.Id.ShouldNotBeNullOrWhiteSpace("Id must be set");
			});
		}

		//PG 10-4-2019 #190 - Updated Code to check that GUID is valid
		private void TheAllergyIntoleranceIdentifierShouldBeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				allergy.Identifier.Count.ShouldBeGreaterThan(0, "There should be at least 1 Identifier system/value pair");
				if (allergy.Identifier.Count == 1)
				{
					var identifier = allergy.Identifier.First();
					identifier.System.ShouldNotBeNullOrWhiteSpace("AllergyIntolerance Identifier System Cannot be null or Empty");
					identifier.Value.ShouldNotBeNullOrEmpty("AllergyIntolerance Identifier Value Cannot be null or Empty Value");
				}
			});
		}

		private void TheAllergyIntoleranceClinicalStatusShouldbeValid()
		{
			ActiveAllergyIntolerances.ForEach(allergy =>
			{
				allergy.ClinicalStatus.Equals(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Active);
			});

			AllAllergyIntolerances.ForEach(allergy =>
			{
				allergy.ClinicalStatus.ShouldNotBeNull("AllergyIntolerance ClinicalStatus cannot be null");
				allergy.ClinicalStatus.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceClinicalStatus>($"AllergyIntolerance ClinicalStatus is not a valid value within the value set {FhirConst.ValueSetSystems.kVsAllergyIntoleranceClinicalStatus}");
				allergy.ClinicalStatus.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Inactive, "The clinical status should never be set to inactive");
			});
		}

		private void TheAllergyIntoleranceVerificationStatusShouldbeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				allergy.VerificationStatus.ShouldNotBeNull("AllergyIntolerance VerificationStatus cannot be null");
				allergy.VerificationStatus.ShouldBe(AllergyIntolerance.AllergyIntoleranceVerificationStatus.Unconfirmed);
			});
		}

		private void TheAllergyIntoleranceCategoryShouldbeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{

				allergy.Category.ShouldNotBeNull("AllergyIntolerance Category cannot be null");
				allergy.Category.ToList().ForEach(category =>
			   {
				   category.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceCategory>($"AllergyIntolerance Category is not a valid value within the value set {FhirConst.ValueSetSystems.kVsAllergyIntoleranceCategory}");
				   category.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceCategory.Biologic);
				   category.ShouldNotBeSameAs(AllergyIntolerance.AllergyIntoleranceCategory.Food);
			   });
			});
		}

		private void TheAllergyIntoleranceCodeShouldbeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				allergy.Code.Coding.ShouldNotBeNull("AllergyIntolerance Code coding cannot be null");

				allergy.Code.Coding.ForEach(coding =>
				{
					coding.System.ShouldNotBeNull("Code should not be null");
				});
			});
		}

		private void TheAllergyIntoleranceAssertedDateShouldBeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				allergy.AssertedDate.ShouldNotBeNullOrEmpty();
			});
		}

		private void TheAllergyIntoleranceEndDateShouldBeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				if (allergy.ClinicalStatus.Equals(AllergyIntolerance.AllergyIntoleranceClinicalStatus.Resolved))
				{
					Extension endAllergy = allergy.GetExtension(FhirConst.StructureDefinitionSystems.kAllergyEndExt);
					endAllergy.ShouldNotBeNull();
					Extension endDate = endAllergy.GetExtension("endDate");
					endDate.ShouldNotBeNull();
					endDate.Value.ShouldNotBeNull();
					Extension endReason = endAllergy.GetExtension("reasonEnded");
					endReason.ShouldNotBeNull();
					endReason.Value.ShouldNotBeNull();
				}
			});
		}

		private void TheAllergyIntolerancePatientShouldBeValidAndResolvable()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				var reference = allergy.Patient.Reference;
				reference.ShouldStartWith("Patient/");
				// git hub ref 159
				// RMB 14/1/19
				//                var resource = _httpSteps.GetResourceForRelativeUrl(GpConnectInteraction.PatientRead, reference);
				//                resource.GetType().ShouldBe(typeof(Patient));
			});
		}

		private void TheAllergyIntoleranceReactionShouldBeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				if (allergy.Reaction != null)
				{
					allergy.Reaction.Count.ShouldBeLessThanOrEqualTo(1);
					// git hub ref 173
					// RMB 4/2/19 - debug for testing code should have been removed
					//                  if (allergy.Reaction.Count == 0)
					//                   {
					//                        allergy.Reaction.Count.ShouldBe(1);
					//                   }               
					// git hub ref 173
					// RMB 23/1/19
					if (allergy.Reaction.Any())
					{
						AllergyIntolerance.ReactionComponent reaction = allergy.Reaction[0];
						if (reaction.Manifestation != null)
						{
							reaction.Manifestation.Count.ShouldBeLessThanOrEqualTo(1);

							if (reaction.Manifestation.Count == 1)
							{
								if (reaction.Severity != null)
								{
									reaction.Severity.ShouldBeOfType<AllergyIntolerance.AllergyIntoleranceSeverity>($"AllergyIntolerance Severity is not a valid value within the value set {FhirConst.ValueSetSystems.kVsAllergyIntoleranceSeverity}");

									var codableConcept = reaction.Manifestation.First();

									var codingDisplay = codableConcept.Coding.First().Display;

									// codingDisplay.ShouldBe("nullFlavor NI", "AllergyIntolerance.reaction.manifestation SHOULD be coded as the nullFlavor NI");

								}
							}
						}

						if (reaction.ExposureRoute != null)
						{
							reaction.ExposureRoute.Coding.First().System.Equals(FhirConst.CodeSystems.kCCSnomed);
						}

						reaction.Note.ShouldBeEmpty();
						reaction.Onset.ShouldBeNull("Allergy Reaction Onset should be Null");
						reaction.Substance.ShouldBeNull("Allergy Reaction Substance should be Null");
					}
				}
			});
		}
		// Added 1.2.0 RMB 15/8/2018

		[Then(@"the Lists are valid for a patient with legacy endReason")]
		public void theListsarevalidforapatientwithlegacyendReason()
		{
			checktheListsarevalidforapatientwithlegacyendReason();
		}

		// Added test for 'No information available' 1.2.0 RMB 7/8/2018
		private void checktheListsarevalidforapatientwithlegacyendReason()
		{
			List<List> resolved = Lists.Where(list => list.Title.Equals(FhirConst.ListTitles.kResolvedAllergies)).ToList();
			if (resolved.Count > 0)
			{
				List<Resource> resolvedAllergies = resolved.First().Contained.Where(resource => resource.ResourceType.Equals(ResourceType.AllergyIntolerance)).ToList();
				resolvedAllergies.ForEach(resource =>
				{
					AllergyIntolerance endedAllergy = (AllergyIntolerance)resource;
					Extension endAllergy = endedAllergy.GetExtension(FhirConst.StructureDefinitionSystems.kAllergyEndExt);
					endAllergy.ShouldNotBeNull();
					Extension endReason = endAllergy.GetExtension("reasonEnded");
					endReason.Value.ShouldNotBeNull();
					endReason.Value.ToString().ShouldBe("No information available");

				});
			}
		}

		// Added 1.2.1 RMB 1/10/2018        
		[Then(@"the AllergyIntolerance Not In Use should be valid")]
		public void TheAllergyIntoleranceNotInUseShouldBeValid()
		{
			AllAllergyIntolerances.ForEach(allergy =>
			{
				allergy.Meta.VersionId.ShouldBeNull("Allergy Meta VersionID should be Null");
				allergy.Meta.LastUpdated.ShouldBeNull("Allergy Meta LastUpdated should be Null");

			});
		}

	}
}
#endregion