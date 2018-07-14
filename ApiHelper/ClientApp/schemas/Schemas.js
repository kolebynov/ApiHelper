import ModelSchema from "./ModelSchema";
import DataTypes from "../common/DataTypes";
import ModelColumnSchema from "./ModelColumnSchema";

const schemas = [
    new ModelSchema({
        name: "Note",
        caption: "Записки",
        primaryColumnName: "id",
        displayColumnName: "name",
        resourceName: "notes",
        columns: [
            new ModelColumnSchema({
                name: "id",
                type: DataTypes.TEXT,
            }),
            new ModelColumnSchema({
                name: "name",
                type: DataTypes.TEXT,
                caption: "Имя"
            }),
            new ModelColumnSchema({
                name: "createdOn",
                type: DataTypes.DATETIME,
                caption: "Дата создания"
            }),
            new ModelColumnSchema({
                name: "modifiedOn",
                type: DataTypes.DATETIME,
                caption: "Дата изменения",
            }),
            new ModelColumnSchema({
                name: "description",
                type: DataTypes.TEXT,
                caption: "Описание",
            }),
            new ModelColumnSchema({
                name: "category",
                type: DataTypes.LOOKUP,
                referenceSchemaName: "NoteCategory",
                caption: "Категория",
                keyColumnName: "categoryId"
            })
        ]
    }),
    new ModelSchema({
        name: "NoteCategory",
        caption: "Категории",
        primaryColumnName: "id",
        displayColumnName: "name",
        resourceName: "categories",
        columns: [
            new ModelColumnSchema({
                name: "id",
                type: DataTypes.TEXT,
            }),
            new ModelColumnSchema({
                name: "name",
                type: DataTypes.TEXT,
                caption: "Имя"
            })
        ]
    }),
    new ModelSchema({
        name: "User",
        caption: "Пользователи",
        primaryColumnName: "id",
        displayColumnName: "id",
        resourceName: "users",
        columns: [
            new ModelColumnSchema({
                name: "id",
                type: DataTypes.TEXT,
            })
        ]
    }),
    new ModelSchema({
        name: "NoteFile",
        caption: "Файлы",
        primaryColumnName: "id",
        displayColumnName: "fileName",
        resourceName: "files",
        columns: [
            new ModelColumnSchema({
                name: "id",
                type: DataTypes.TEXT
            }),
            new ModelColumnSchema({
                name: "contentType",
                type: DataTypes.TEXT,
                caption: "Тип контента"
            }),
            new ModelColumnSchema({
                name: "fileName",
                type: DataTypes.TEXT,
                caption: "Имя файла"
            }),
            new ModelColumnSchema({
                name: "length",
                type: DataTypes.NUMBER,
                caption: "Размер"
            }),
            new ModelColumnSchema({
                name: "note",
                type: DataTypes.LOOKUP,
                caption: "Записка",
                referenceSchemaName: "Note",
                keyColumnName: "noteId"
            })
        ]
    }),
    new ModelSchema({
        name: "NoteComment",
        caption: "Комментарии",
        primaryColumnName: "id",
        displayColumnName: "Text",
        resourceName: "comments",
        columns: [
            new ModelColumnSchema({
                name: "id",
                type: DataTypes.TEXT
            }),
            new ModelColumnSchema({
                name: "сreatedOn",
                type: DataTypes.DATETIME,
                caption: "Дата создания"
            }),
            new ModelColumnSchema({
                name: "modifiedOn",
                type: DataTypes.DATETIME,
                caption: "Дата модификации"
            }),
            new ModelColumnSchema({
                name: "note",
                type: DataTypes.LOOKUP,
                caption: "Записка",
                referenceSchemaName: "Note",
                keyColumnName: "noteId"
            }),
            new ModelColumnSchema({
                name: "text",
                caption: "Комментарий",
                type: DataTypes.TEXT
            }),
        ]
    })
];

export default schemas;