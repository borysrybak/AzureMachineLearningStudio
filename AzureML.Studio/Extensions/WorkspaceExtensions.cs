﻿using AzureML.Studio.Core.Enums;
using AzureML.Studio.Core.Models;
using AzureML.Studio.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AzureML.Studio.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class WorkspaceExtensions
    {
        private static readonly ManagementService _managementService;
        static WorkspaceExtensions()
        {
            _managementService = ManagementService.Instance;
        }

        /// <summary>
        /// Get Users of Azure Machine Learning Studio workspace.
        /// </summary>
        /// <param name="workspace">Required parameter to get workspace users.</param>
        /// <returns>Returns collection of users from that particular workspace.</returns>
        public static IEnumerable<WorkspaceUser> GetUsers(this Workspace workspace)
        {
            return _managementService.GetWorkspaceUsers(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            });
        }

        /// <summary>
        /// Add new user to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        public static void AddUser(this Workspace workspace, string email, string role)
        {
            _managementService.AddWorkspaceUsers(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            },
                email, role);
        }

        /// <summary>
        /// Add new user to workspace. 
        /// </summary>
        /// <param name="workspace">Required parameter to add new user to specific workspace.</param>
        /// <param name="workspaceUser">Required parameter to add new user profile.</param>
        public static void AddUser(this Workspace workspace, WorkspaceUser workspaceUser)
        {
            AddUser(workspace, workspaceUser.Email, workspaceUser.Role);
        }

        /// <summary>
        /// Add new users to workspace. 
        /// </summary>
        /// <param name="workspace">Required parameter to add new user to specific workspace.</param>
        /// <param name="workspaceUsers">Required parameter to add new user profile.</param>
        public static void AddUsers(this Workspace workspace, IEnumerable<WorkspaceUser> workspaceUsers)
        {
            workspaceUsers.ForEach(wu => AddUser(workspace, wu));
        }

        /// <summary>
        /// Get datasets from workspace.
        /// </summary>
        /// <param name="workspace">Required parameter to get dataset from this parcticular workspace.</param>
        /// <returns>Returns dataset collection.</returns>
        public static IEnumerable<Dataset> GetDatasets(this Workspace workspace)
        {
            return _managementService.GetDatasets(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            });
        }

        /// <summary>
        /// Delete dataset from workspace
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetFamilyId"></param>
        public static void DeleteDataset(this Workspace workspace, string datasetFamilyId)
        {
            _managementService.DeleteDataset(
                 new WorkspaceSettings()
                 {
                     WorkspaceId = workspace.WorkspaceId,
                     AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                     Location = workspace.Region
                 },
                 datasetFamilyId);
        }

        /// <summary>
        /// Delete dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="dataset"></param>
        public static void DeleteDataset(this Workspace workspace, Dataset dataset)
        {
            DeleteDataset(workspace, dataset.FamilyId);
        }

        /// <summary>
        /// Delete datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetsFamilyIds"></param>
        public static void DeleteDatasets(this Workspace workspace, IEnumerable<string> datasetsFamilyIds)
        {
            datasetsFamilyIds.ForEach(dfi => DeleteDataset(workspace, dfi));
        }

        /// <summary>
        /// Delete datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasets"></param>
        public static void DeleteDatasets(this Workspace workspace, IEnumerable<Dataset> datasets)
        {
            DeleteDatasets(workspace, datasets.Select(d => d.FamilyId));
        }

        /// <summary>
        /// Delete all datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        public static void DeleteAllDatasets(this Workspace workspace)
        {
            DeleteDatasets(workspace, GetDatasets(workspace));
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetId"></param>
        /// <param name="fileName"></param>
        public static void DownloadDataset(this Workspace workspace, string datasetId, string fileName = "dataset")
        {
            _managementService.DownloadDatasetAsync(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            }, datasetId, $"{fileName}.{workspace.WorkspaceId}.{datasetId}");
        }

        /// <summary>
        /// Download dataset from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="dataset"></param>
        /// <param name="fileName"></param>
        public static void DownloadDataset(this Workspace workspace, Dataset dataset, string fileName = "dataset")
        {
            DownloadDataset(workspace, dataset.Id, fileName);
        }

        /// <summary>
        /// Download selected datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasetsIds"></param>
        public static void DownloadDatasets(this Workspace workspace, IEnumerable<string> datasetsIds)
        {
            datasetsIds.ForEach(di => DownloadDataset(workspace, di));
        }

        /// <summary>
        /// Download selected datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="datasets"></param>
        public static void DownloadDatasets(this Workspace workspace, IEnumerable<Dataset> datasets)
        {
            datasets.ForEach(d => DownloadDataset(workspace, d));
        }

        /// <summary>
        /// Download all datasets from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        public static void DownloadAllDatasets(this Workspace workspace)
        {
            DownloadDatasets(workspace, GetDatasets(workspace));
        }

        /// <summary>
        /// Upload resource to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="resourceFileFormat"></param>
        /// <param name="filePath"></param>
        public static async void UploadResource(this Workspace workspace, ResourceFileFormat resourceFileFormat, string filePath = "dataset")
        {
            await _managementService.UploadResourceAsync(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            }, resourceFileFormat.GetDescription(), filePath);
        }

        /// <summary>
        /// Upload resources to workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="resources"></param>
        public static void UploadResources(this Workspace workspace, IDictionary<string, ResourceFileFormat> resources)
        {
            resources.ForEach(pair => UploadResource(workspace, pair.Value, pair.Key));
        }

        /// <summary>
        /// Get experiments from workspace.
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns>Returns experiments collection from workspace.</returns>
        public static IEnumerable<Experiment> GetExperiments(this Workspace workspace)
        {
            return _managementService.GetExperiments(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            });
        }

        /// <summary>
        /// Get experiment by id.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        /// <returns>Returns experiment from workspace.</returns>
        public static Experiment GetExperiment(this Workspace workspace, string experimentId)
        {
            var rawJson = string.Empty;
            return _managementService.GetExperimentById(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            }, experimentId, out rawJson);
        }

        /// <summary>
        /// Get experiments by ids.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        /// <returns>Returns experiments collection from workspace.</returns>
        public static IEnumerable<Experiment> GetExperiments(this Workspace workspace, IEnumerable<string> experimentsIds)
        {
            return experimentsIds.Select(ei => GetExperiment(workspace, ei));
        }

        /// <summary>
        /// Run experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        public static void RunExperiment(this Workspace workspace, string experimentId)
        {
            var rawJson = string.Empty;
            var workspaceSettings = new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            };
            var experiment = _managementService.GetExperimentById(workspaceSettings, experimentId, out rawJson);
            _managementService.RunExperiment(
                workspaceSettings,
                experiment,
                rawJson);
        }

        /// <summary>
        /// Run experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        public static void RunExperiment(this Workspace workspace, Experiment experiment)
        {
            RunExperiment(workspace, experiment.ExperimentId);
        }

        /// <summary>
        /// Run experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        public static void RunExperiments(this Workspace workspace, IEnumerable<string> experimentsIds)
        {
            experimentsIds.ForEach(ei => RunExperiment(workspace, ei));
        }

        /// <summary>
        /// Run experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiments"></param>
        public static void RunExperiments(this Workspace workspace, IEnumerable<Experiment> experiments)
        {
            experiments.ForEach(e => RunExperiment(workspace, e));
        }

        /// <summary>
        /// Run all experiments.
        /// </summary>
        /// <param name="workspace"></param>
        public static void RunExperiments(this Workspace workspace)
        {
            RunExperiments(workspace, GetExperiments(workspace));
        }

        /// <summary>
        /// Save experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        public static void SaveExperiment(this Workspace workspace, string experimentId)
        {
            var rawJson = string.Empty;
            var workspaceSettings = new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            };
            var experiment = _managementService.GetExperimentById(workspaceSettings, experimentId, out rawJson);
            _managementService.SaveExperiment(workspaceSettings, experiment, rawJson);
        }

        /// <summary>
        /// Save experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        public static void SaveExperiment(this Workspace workspace, Experiment experiment)
        {
            SaveExperiment(workspace, experiment.ExperimentId);
        }

        /// <summary>
        /// Save experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        public static void SaveExperiments(this Workspace workspace, IEnumerable<string> experimentsIds)
        {
            experimentsIds.ForEach(ei => SaveExperiment(workspace, ei));
        }

        /// <summary>
        /// Save experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiments"></param>
        public static void SaveExperiments(this Workspace workspace, IEnumerable<Experiment> experiments)
        {
            experiments.ForEach(e => SaveExperiment(workspace, e));
        }

        /// <summary>
        /// Save all experiments.
        /// </summary>
        /// <param name="workspace"></param>
        public static void SaveExperiments(this Workspace workspace)
        {
            SaveExperiments(workspace, GetExperiments(workspace));
        }

        /// <summary>
        /// Save experiment as.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        /// <param name="newName"></param>
        public static void SaveExperimentAs(this Workspace workspace, string experimentId, string newName)
        {
            var rawJson = string.Empty;
            var workspaceSettings = new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            };
            var experiment = _managementService.GetExperimentById(workspaceSettings, experimentId, out rawJson);
            _managementService.SaveExperimentAs(workspaceSettings, experiment, rawJson, newName);
        }

        /// <summary>
        /// Save experiment as.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        /// <param name="newName"></param>
        public static void SaveExperimentAs(this Workspace workspace, Experiment experiment, string newName)
        {
            SaveExperimentAs(workspace, experiment.ExperimentId, newName);
        }

        /// <summary>
        /// Delete experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        public static void DeleteExperiment(this Workspace workspace, string experimentId)
        {
            _managementService.RemoveExperimentById(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            }, experimentId);
        }

        /// <summary>
        /// Delete experiment.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        public static void DeleteExperiment(this Workspace workspace, Experiment experiment)
        {
            DeleteExperiment(workspace, experiment.ExperimentId);
        }

        /// <summary>
        /// Delete experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        public static void DeleteExperiments(this Workspace workspace, IEnumerable<string> experimentsIds)
        {
            experimentsIds.ForEach(ei => DeleteExperiment(workspace, ei));
        }

        /// <summary>
        /// Delete experiments.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiments"></param>
        public static void DeleteExperiments(this Workspace workspace, IEnumerable<Experiment> experiments)
        {
            experiments.ForEach(e => DeleteExperiment(workspace, e));
        }

        /// <summary>
        /// Delete all experiments.
        /// </summary>
        /// <param name="workspace"></param>
        public static void DeleteExperiments(this Workspace workspace)
        {
            DeleteExperiments(workspace, GetExperiments(workspace));
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        public static void ExportExperiment(this Workspace workspace, string experimentId)
        {
            var rawJson = string.Empty;
            var outputFile = _managementService.GetExperimentById(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            }, experimentId, out rawJson);
            File.WriteAllText(outputFile.ExperimentId, rawJson);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentId"></param>
        /// <param name="outputFile"></param>
        public static void ExportExperiment(this Workspace workspace, string experimentId, string outputFile)
        {
            var rawJson = string.Empty;
            _managementService.GetExperimentById(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            }, experimentId, out rawJson);
            File.WriteAllText(outputFile, rawJson);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        public static void ExportExperiment(this Workspace workspace, Experiment experiment)
        {
            ExportExperiment(workspace, experiment.ExperimentId);
        }

        /// <summary>
        /// Export experiment as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiment"></param>
        /// <param name="outputFile"></param>
        public static void ExportExperiment(this Workspace workspace, Experiment experiment, string outputFile)
        {
            ExportExperiment(workspace, experiment.ExperimentId, outputFile);
        }

        /// <summary>
        /// Export specific experiments as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experimentsIds"></param>
        public static void ExportExperiments(this Workspace workspace, IEnumerable<string> experimentsIds)
        {
            experimentsIds.ForEach(ei => ExportExperiment(workspace, ei));
        }

        /// <summary>
        /// Export specific experiments as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="experiments"></param>
        public static void ExportExperiments(this Workspace workspace, IEnumerable<Experiment> experiments)
        {
            experiments.ForEach(e => ExportExperiment(workspace, e));
        }

        /// <summary>
        /// Export all experiments as JSON.
        /// </summary>
        /// <param name="workspace"></param>
        public static void ExportExperiments(this Workspace workspace)
        {
            ExportExperiments(workspace, GetExperiments(workspace));
        }

        //TODO:
        //public static void ImportExperiment(this Workspace workspace, string inputFile)
        //{

        //}

        //public static void ImportExperiment(this Workspace workspace, string inputFile, string newName)
        //{

        //}

        /// <summary>
        /// Get trained model.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="userAssetId"></param>
        /// <returns></returns>
        public static UserAsset GetTrainedModel(this Workspace workspace, string userAssetId)
        {
            return GetTrainedModels(workspace).First(tm => tm.Id.Equals(userAssetId));
        }

        /// <summary>
        /// Get trained models.
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public static IEnumerable<UserAsset> GetTrainedModels(this Workspace workspace)
        {
            return _managementService.GetTrainedModels(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            });
        }

        /// <summary>
        /// Get transform.
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="userAssetId"></param>
        /// <returns></returns>
        public static UserAsset GetTransform(this Workspace workspace, string userAssetId)
        {
            return GetTransforms(workspace).First(tm => tm.Id.Equals(userAssetId));
        }

        /// <summary>
        /// Get transforms.
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public static IEnumerable<UserAsset> GetTransforms(this Workspace workspace)
        {
            return _managementService.GetTransforms(new WorkspaceSettings()
            {
                WorkspaceId = workspace.WorkspaceId,
                AuthorizationToken = workspace.AuthorizationToken.PrimaryToken,
                Location = workspace.Region
            });
        }
    }
}
