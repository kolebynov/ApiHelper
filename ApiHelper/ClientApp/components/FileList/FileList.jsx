import React from "react";
import propTypes from "prop-types";
import { 
    Table,
    TableBody,
    TableHeader,
    TableHeaderColumn,
    TableRow,
    TableRowColumn
} from "material-ui/Table";
import urlHelper from "../../utils/UrlHelper";
import modelUtils from "../../utils/ModelUtils";
import modelSchemaProvider from "../../schemas/ModelSchemaProvider";
import constants from "../../utils/Constants";
import ApiService from "../../services/ApiService";
import FlatButton from 'material-ui/FlatButton';
import noteFileService from "../../services/NoteFileService";

class FileList extends React.PureComponent {
    constructor() {
        super();

        this.state = {
            files: []
        }

        this._fileSchema = modelSchemaProvider.getSchemaByName("NoteFile");
        this._noteSchema = modelSchemaProvider.getSchemaByName("Note");
    }

    componentWillMount() {
        this._loadFiles();
    }

    render() {
        if (!this.props.noteId || this.props.noteId === constants.EMPTY_GUID) {
            return null;
        }

        return (
            <div>
                <span>Файлы</span>
                <div>
                    <FlatButton label="Добавить" onClick={this._onButtonAddClick}/>
                </div>
                <Table>
                    <TableHeader adjustForCheckbox={false} displaySelectAll={false}>
                        <TableRow>
                            <TableHeaderColumn>Имя файла</TableHeaderColumn>
                            <TableHeaderColumn>Размер</TableHeaderColumn>
                        </TableRow>
                    </TableHeader>
                    <TableBody displayRowCheckbox={false}>
                        {this.state.files.map(file => (
                            <TableRow key={modelUtils.getPrimaryValue(file, this._fileSchema)}>
                                <TableRowColumn>
                                    <a href={urlHelper.getDownloadFileUrl(modelUtils.getPrimaryValue(file, this._fileSchema))}>
                                        {modelUtils.getDisplayValue(file, this._fileSchema)}
                                    </a>
                                </TableRowColumn>
                                <TableRowColumn>{file.length}</TableRowColumn>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </div>
        );
    }

    _loadFiles() {
        new ApiService(this._noteSchema.resourceName).getItems(this.props.noteId, null, this._fileSchema.resourceName)
            .then(response => this.setState({
                files: response.data
            }));
    }

    _onButtonAddClick = () => {
        const fileInput = document.createElement("input");
        fileInput.setAttribute("type", "file");
        fileInput.setAttribute("multiple", null);
        fileInput.addEventListener("change", () => {
            noteFileService.uploadFiles(this.props.noteId, fileInput.files)
                .then(() => this._loadFiles());
        });
        fileInput.click();
    }
}

FileList.propTypes = {
    noteId: propTypes.string.isRequired
};

export default FileList;